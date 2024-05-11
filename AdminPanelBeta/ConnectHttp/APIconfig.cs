using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AdminPanelBeta.ConnectHttp
{

    public class APIConfig
    {
        public static string BaseUrl { get; set; } = "http://evseev-dv.tepk-it.ru/api";
    }
}