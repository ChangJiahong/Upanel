using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HookLock
{
    class Email
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="发件人">发件人</param>
        /// <param name="收件人">收件人</param>
        /// <param name="授权码">授权码</param>
        /// <param name="Subjiect">邮件标题</param>
        /// <param name="msg">邮件正文</param>
        /// <param name="images">邮件图片附件</param>
        public static void send_Email(String 发件人, String 收件人, String 授权码, String Subjiect, String msg, String[] images)
        {
            MailMessage myMail = new MailMessage();//创建邮件实例对象


            myMail.From = new MailAddress(发件人);//发送者，要和邮件服务器的验证信息对应，不能随便更改
            myMail.To.Add(new MailAddress(收件人)); //接收者

            myMail.Subject = Subjiect; //邮件标题
            myMail.SubjectEncoding = Encoding.UTF8;//标题编码

            myMail.Body = msg;//邮件内容
            myMail.IsBodyHtml = true; //邮件内容是否支持html


            for (int i = 0; i < images.Length; i++)
            {

                System.Net.Mail.Attachment objFile = new System.Net.Mail.Attachment(images[i]);
                objFile.Name = "图片" + (i + 1) + ".jpg";
                myMail.Attachments.Add(objFile);
            }

            myMail.BodyEncoding = Encoding.UTF8; //邮件内容编码

            SmtpClient smtp = new SmtpClient();//创建smtp实例对象
            smtp.Host = "smtp.qq.com";//邮件服务器SMTP

            //smtp.Port = 25; //邮件服务器端口
            //SMTP端口，QQ邮箱填写587  
            smtp.Port = 587;
            //启用SSL加密  
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(发件人, 授权码); //邮件服务器验证信息

            smtp.Send(myMail);//发送邮件
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="发件人">发件人</param>
        /// <param name="收件人">收件人</param>
        /// <param name="授权码">授权码</param>
        /// <param name="Subjiect">邮件标题</param>
        /// <param name="msg">邮件内容</param>
        public static void send_Email(String 发件人, String 收件人, String 授权码, String Subjiect, String msg)
        {
            MailMessage myMail = new MailMessage();//创建邮件实例对象


            myMail.From = new MailAddress(发件人);//发送者，要和邮件服务器的验证信息对应，不能随便更改
            myMail.To.Add(new MailAddress(收件人)); //接收者

            myMail.Subject = Subjiect; //邮件标题
            myMail.SubjectEncoding = Encoding.UTF8;//标题编码

            myMail.Body = msg;//邮件内容
            myMail.IsBodyHtml = true; //邮件内容是否支持html


            myMail.BodyEncoding = Encoding.UTF8; //邮件内容编码

            SmtpClient smtp = new SmtpClient();//创建smtp实例对象
            smtp.Host = "smtp.qq.com";//邮件服务器SMTP

            //smtp.Port = 25; //邮件服务器端口
            //SMTP端口，QQ邮箱填写587  
            smtp.Port = 587;
            //启用SSL加密  
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(发件人, 授权码); //邮件服务器验证信息

            smtp.Send(myMail);//发送邮件
        }

    }
}
