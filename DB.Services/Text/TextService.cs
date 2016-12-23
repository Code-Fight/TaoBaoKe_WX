using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DB.Entity;
using DB.Services.DBbase;
using HttpCodeLib;
using TB.Services;

namespace DB.Services.Text
{
    /// <summary>
    /// 处理跟文字相关的
    /// </summary>
    public class TextService
    {
        /// <summary>
        /// 根据用户分享过来的链接查找优惠券
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetTKlink(string msg)
        {
            try
            {
                var regList = GetRegexStr(msg,
                    @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"
                    );
                if (regList.Count <= 0)
                {
                    return "抱歉，无法解析,请发送正常分享链接";
                }
                var realLink = string.Empty;
                var itemId = string.Empty;
                if (!regList[0].Contains("taobao") && !regList[0].Contains("tmall"))
                {
                    regList = GetRegexStr(GetRealLink(regList[0]), "var url = '(.*?)'");

                    if (regList.Count <= 0)
                    {
                        return "抱歉，该链接已经失效或不存在";
                    }
                    realLink = regList[1];

                    if (!realLink.Contains("taobao") && !realLink.Contains("tmall"))
                    {
                        return "抱歉，目前只支持淘宝或者天猫";
                    }
                    
                    if (GetRegexStr(realLink, "&id=[1-9][0-9]{0,99}").Count > 0)
                    {
                        regList = GetRegexStr(realLink, "&id=[1-9][0-9]{0,99}");
                        itemId = regList[1].Replace("id=", "");
                    }
                    else if (GetRegexStr(realLink, "i(.*?).htm").Count > 0)
                    {
                        regList = GetRegexStr(realLink, "i(.*?).htm");
                        itemId = regList[1];
                    }
                }
                else
                {
                    itemId = GetRegexStr(regList[0], "id=[1-9][0-9]{6,99}")[0].Replace("id=", "");
                }


               


                if (regList.Count <= 0 || itemId.Trim().Length<=0)
                {
                    return "抱歉，解析失败，请重试";
                }


                var ret =
                    DbExcel.Context.From<tb_excel>()
                        .Where(tb_excel._.item_id.Trim() == itemId.Trim())
                        .ToFirst();


                if (ret==null||ret.quan_tg_url.Length<=0)
                {
                    return "暂未找到该商品的优惠券，可以使用 @搜 “关键字”,来查找类似产品的优惠券。比如回复：@搜 耳机，即可返回耳机相关的优惠信息";
                }

                //转换为淘口令
                var klStr= TaoKouL.Create(ret.item_name, ret.item_img, ret.quan_tg_url);

                var retMsg = "优惠券找到啦！复制本条消息，打开手机淘宝，立即查看即可！"+klStr;
                return retMsg;

            }
            catch (Exception)
            {
                return "抱歉，系统异常，请稍后重试";
            }
        }






        /// <summary>
        /// 正则表达式获取文本结果
        /// </summary>
        /// <param name="reString">请替换为需要处理的字符串</param>
        /// <returns>处理结果</returns>
        private List<string> GetRegexStr(string reString, string regStr)
        {
            System.Text.RegularExpressions.Regex reg;//正则表达式变量
            //注意 reString 请替换为需要处理的字符串
            List<string> strList = new List<string>();
            string regexCode = regStr;
            reg = new System.Text.RegularExpressions.Regex(regexCode);
            System.Text.RegularExpressions.MatchCollection mc = reg.Matches(reString);
            for (int i = 0; i < mc.Count; i++)
            {
                GroupCollection gc = mc[i].Groups; //得到所有分组 
                for (int j = 0; j < gc.Count; j++) //多分组 匹配的原始文本不要
                {
                    string temp = gc[j].Value;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        strList.Add(temp); //获取结果   strList中为匹配的值
                    }
                }
            }
            //需要获取匹配的数据,请遍历strList  通常情况下(正则表达式中只有一个分组),只需要取strList[1]即可. 如果有多个分组,依次类推即可.
            return strList;
        }


        /// <summary>
        /// 执行Http请求方法
        /// </summary>
        public string GetRealLink(string _url)
        {
            HttpHelpers helper = new HttpHelpers();//请求执行对象
            HttpItems items;//请求参数对象
            HttpResults hr = new HttpResults();//请求结果对象
            string StrCookie = "";//设置初始Cookie值
            string res = string.Empty;//请求结果,请求类型不是图片时有效
            string url = _url;//请求地址
            items = new HttpItems();//每次重新初始化请求对象
            items.URL = url;//设置请求地址
            items.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";//设置UserAgent
            items.Cookie = StrCookie;//设置字符串方式提交cookie
            items.Allowautoredirect = true;//设置自动跳转(True为允许跳转) 如需获取跳转后URL 请使用 hr.RedirectUrl
            items.ContentType = "application/x-www-form-urlencoded";//内容类型
            hr = helper.GetHtml(items, ref StrCookie);//提交请求
            res = hr.Html;//具体结果
            return res;//返回具体结果
        }



    }
}
