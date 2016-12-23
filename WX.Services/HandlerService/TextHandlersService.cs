using System.Text;
using DB.Services.Text;
using Senparc.Weixin.MP.Entities;

namespace WX.Services.HandlerService
{
    /// <summary>
    /// 文本消息处理
    /// </summary>
    public class TextHandlersService
    {
        public ResponseMessageText GetResponseMessage(RequestMessageText requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);


            if (requestMessage.Content.Contains("@帮助"))
            {
                var result = new StringBuilder();
                result.AppendFormat("感谢您使用帮帮熊优惠券发布系统，使用方式如下：\r\n");
                result.AppendFormat("1.发送命令【@分类】可以获取分类整理后的优惠券信息\r\n");
                result.AppendFormat("2.通过发送命令具体类目命令，比如【@服装】可以获取服装类优惠券信息，目前支持的类目如下：\r\n");
                result.AppendFormat("@服装|@母婴|@居家|@美食|@化妆品|@鞋包配饰|@文体车品|@数码家电  \r\n");
                result.AppendFormat("3.查询特定商品宝贝，请看此处：【链接】  \r\n");
                result.AppendFormat("4.发送【@客服】可获取客服信息进行人工服务\r\n");
                result.AppendFormat("5.更多功能开发中，敬请期待");
                responseMessage.Content = result.ToString();
            }
            else if (requestMessage.Content.Contains("@分类"))
            {
                var result = new StringBuilder();
                result.AppendFormat("感谢您使用帮帮熊优惠券发布系统，使用方式如下：\r\n");
                result.AppendFormat("服装:http://t.cn/RIUfG42 \r\n");
                result.AppendFormat("母婴:http://t.cn/RIUfVq5 \r\n");
                result.AppendFormat("居家:http://t.cn/RIUfiUG \r\n");
                result.AppendFormat("美食:http://t.cn/RIUfKNr  \r\n");
                result.AppendFormat("化妆品:http://t.cn/RIUfMtW \r\n");
                result.AppendFormat("鞋包配饰:http://t.cn/RIUfa4G \r\n");
                result.AppendFormat("文体车品:http://t.cn/RIUfNUn \r\n");
                result.AppendFormat("数码家电:http://t.cn/RIUf03S \r\n");
                responseMessage.Content = result.ToString();
            }
            else if (requestMessage.Content.Contains("@服装"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfG42"">服装类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@母婴"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfVq5"">母婴类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@居家"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfiUG"">居家类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@美食"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfKNr"">美食类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@化妆品"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfMtW"">化妆品类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@鞋包配饰"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfa4G"">鞋包配饰类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@文体车品"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUfNUn"">文体车品类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@数码家电"))
            {
                responseMessage.Content = @"<a href=""http://t.cn/RIUf03S"">数码家电类精品优惠券</a>";
            }
            else if (requestMessage.Content.Contains("@客服"))
            {
                responseMessage.Content = @"客服微信号：XXXXXX";
            }
            else if (requestMessage.Content.Contains("http"))
            {
                responseMessage.Content = new TextService().GetTKlink(requestMessage.Content);
            }
            else
            {
                responseMessage.Content = @"未知命令，发送命令时无需带“【】”，如需帮助请回复 @帮助";
            }

            return responseMessage;
        }
    }
}