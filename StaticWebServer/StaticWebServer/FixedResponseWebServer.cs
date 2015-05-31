using System;
using System.Net;
using System.Threading;

namespace StaticWebServer
{
    public class FixedResponseWebServer
    {
        public string Endpoint { get; private set; }

        private HttpListener HttpListener { get; set; }
        private static readonly object HttpListenerLock = new object();

        private readonly ResourceFile _resourceFile;

        public FixedResponseWebServer(string endpoint, string path, string contentType = null)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/json";
            }

            Endpoint = endpoint;

            HttpListener = new HttpListener();
            HttpListener.Prefixes.Add(endpoint);

            _resourceFile = new ResourceFile(path, contentType);
        }

        public void Start()
        {
            lock (HttpListenerLock)
            {
                if (!HttpListener.IsListening)
                {
                    HttpListener.Start();
                }
            }

            HandleRequest();
        }

        private void HandleRequest()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("FixedResponseWebServer running...");
                try
                {
                    while (HttpListener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(context =>
                        {
                            var httpListenerContext = context as HttpListenerContext;
                            if(httpListenerContext == null)
                                return;

                            try
                            {
                                var response = _resourceFile.ByteArray;
                                httpListenerContext.Response.ContentType = _resourceFile.ContentType;
                                httpListenerContext.Response.ContentLength64 = response.LongLength;
                                httpListenerContext.Response.OutputStream.WriteAsync(response, 0, response.Length);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                httpListenerContext.Response.OutputStream.Close();
                            }
                        }, HttpListener.GetContext());
                    }
                }
                catch
                {
                    // ignored
                }
            });
        }

        public void Stop()
        {
            lock (HttpListenerLock)
            {
                if (HttpListener == null)
                    return;
                HttpListener.Stop();
                HttpListener.Close();
            }
        }


    }
}
