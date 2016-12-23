using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace TB.Services
{
    public class TaoKouL
    {
        /// <summary>
        /// 创建淘口令
        /// </summary>
        /// <param name="text">提示文本</param>
        /// <param name="logo">logo地址</param>
        /// <param name="url">要转换的地址</param>
        /// <returns></returns>
        public static string Create(string text, string logo,string url) 
        {
            ClusterTopClient client = new ClusterTopClient("http://gw.api.taobao.com/router/rest", "23493845", "bd0af6f3badd3721152b139910bb5124", "json");
            //ITopClient client = new DefaultTopClient(url, appkey, secret);
            WirelessShareTpwdCreateRequest req = new WirelessShareTpwdCreateRequest();
            WirelessShareTpwdCreateRequest.IsvTpwdInfoDomain obj1 = new WirelessShareTpwdCreateRequest.IsvTpwdInfoDomain();
            obj1.Ext = "{\"xx\":\"xx\"}";
            obj1.Logo = logo;
            obj1.Text = text;
            obj1.Url = url;
            obj1.UserId = 24234234234L;
            req.TpwdParam_ = obj1;
            WirelessShareTpwdCreateResponse rsp = client.Execute(req);
            if (!rsp.IsError)
            {
                return rsp.Model;
            }
            return string.Empty;
        }
    }
}
