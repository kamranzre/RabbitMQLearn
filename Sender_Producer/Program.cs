using System;
using System.Text;
using RabbitMQ.Client;
namespace Sender_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = RabbitMQConfig.Instance.CreateConnection();
            var channel = RabbitMQConfig.Instance.CreateModel(connection);

            //ایجاد یک صف
            channel.QueueDeclare("myQueue01", true, false, true, null);
            //QueueDeclare پارامتر های متد 
            //1==>نام صف
            //2==durable==>بودن صف در هارد ذخیره میشود تا در صورت استاپ شدن سرویس ربیت صف در هارد نگهداری شود و بعد از فعال سازی سرویس دوباره صف  اضافه شود true در صورت
            //3==exvlusive==> Consumer حذف صف بعد از بسته شدن کانکشن 
            //4==autoDelete==>صف به صورت خودکار حذف میشود
            for (int i = 0; i < 3; i++)
            {
                var body = Encoding.UTF8.GetBytes($"Test1 {i}");

                #region Persistent
                //  RabbitMQ با این کار اطلاعات موجود در صف را در هارد نگهداری میکنیم تا درصورتی که سرویس
                // به هر دلیلی استاپ شد دیتا ها در هارد نگه داری شوند.
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                #endregion


                channel.BasicPublish("", "myQueue01", properties, body);
            }

            //باید دیتا را به بایت تبدیل کنیم برای ارسال

            //پیغام به این روش ارسال میشود

            channel.Close();
            connection.Close();
            Console.ReadKey();
        }
    }
}
