using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            //ömer https://www.c-sharpcorner.com/article/using-net-core-with-rabbitmq-for-async-operations/ 


            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://dazfzfmg:HLo6r8xAKPluHAqQE94tMj-PgWweHUAt@grouse.rmq.cloudamqp.com/dazfzfmg");


            ////factory.HostName = "localhost";
            ////factory.UserName = "guest";
            ////factory.Password = "@mAT23ok";
            ///
            string queueName = "task_queue";
            var rabbitMqConnection = factory.CreateConnection();
            var rabbitMqChannel = rabbitMqConnection.CreateModel();

            rabbitMqChannel.QueueDeclare(queue: queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

            rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            int messageCount = Convert.ToInt16(rabbitMqChannel.MessageCount(queueName));
            Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {

                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(" Mesaj Alındı: " + message);

                rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);


                
                Thread.Sleep(1000);

            };
            rabbitMqChannel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);


            Console.ReadLine();


            //string queueName = "task_queue";

            //var factory = new ConnectionFactory();//instance bağlanmak için oluşturduk
            //factory.Uri = new Uri("amqps://dazfzfmg:HLo6r8xAKPluHAqQE94tMj-PgWweHUAt@grouse.rmq.cloudamqp.com/dazfzfmg");


            ////factory.HostName = "localhost";
            ////factory.UserName = "guest";
            ////factory.Password = "@mAT23ok";

            ////bağlantıyı açalım.
            ////usin ifadesi içerisinde bir clasdan bir nesne oluşturursak 
            ////ve o nesnemiz idisposable interfacesine sahipse using bloğundan çıktığında bu arkadaş memordiden silinir
            //using (var connection = factory.CreateConnection())
            //{
            //    //bağlantı hazırsa kanal oluşturalım
            //    using (var channel = connection.CreateModel())
            //    {

            //        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, null);

            //        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            //        //prefectSize : 
            //        //1 : bana bir bir dağıt. 2 dersem iki iki yollar
            //        //false : her consumer 1 tane queue alabilir.  diyelim 10 tane consumer var. 10 mesaj alır.
            //        //true olursa 10 consumer toplam da 1 tane mesaj alır. 




            //        var consumer = new EventingBasicConsumer(channel);//bu kanalı dinle
            //        consumer.Received += (model, ea) =>
            //        {

            //            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            //            Console.WriteLine("Mesaj Aldın" + message);





            //            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);//okundu mesajı

            //            int time = 100;// int.Parse(GetMessages(args));
            //            Thread.Sleep(time);


            //        };

            //        channel.BasicConsume(queue: queueName, autoAck: false, consumer);
            //        //autoAck : true verirsek doğruda yanlışda işlesek rabbitmq dan silinecek. false dersek biz bilgi vericez öle sil diyecez




            //    }



            //}
            //Console.ReadLine();
        }

      
    }
}
