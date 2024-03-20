using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Reciver_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = RabbitMQConfig.Instance.CreateConnection();
            var channel = RabbitMQConfig.Instance.CreateModel(connection);
            channel.QueueDeclare("myQueue01", true, false, true, null);

            #region BasicQos
            channel.BasicQos(0, 1, false);//Fair dispatch
            //کار ها به صورت تکی به هر کدام از سرور ها ارسال میشود تا بعد از 
            //انجام کارشان کار دیگری به ان ها داده شود
            //با این کار سربار بر اساس قدرت هر سرور تقسیم میشود
            //تا در صورت سرعتی تر بودن یک سرور کار های بیشتری به او سپرده شود
            #endregion

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArg) =>
            {
                var body = eventArg.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received Message " + message);

                channel.BasicAck(eventArg.DeliveryTag, true);//با این دستور میتوانید به صورت دستی مشخص کنید که دیتا دریافت شده و هندل نیز شده و شناسه پیغام را به
                // ارسال میکنید تا آن را از صف حذف کندRabbit
            };
            channel.BasicConsume("myQueue01", false, consumer);
            Console.ReadKey();
        }
    }
}
