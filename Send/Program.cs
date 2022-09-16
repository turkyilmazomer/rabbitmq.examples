using RabbitMQ.Client;
using System;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();//instance bağlanmak için oluşturduk
            factory.Uri = new Uri("amqps://dazfzfmg:HLo6r8xAKPluHAqQE94tMj-PgWweHUAt@grouse.rmq.cloudamqp.com/dazfzfmg");

            //factory.HostName = "localhost";
            //factory.UserName = "####KullanıcıAdıGir"####";
            //factory.Password = "####ŞifreyiGir"####


            //bağlantıyı açalım.
            //using ifadesi içerisinde bir clasdan bir nesne oluşturursak 
            //ve o nesnemiz idisposable interfacesine sahipse using bloğundan çıktığında bu arkadaş memordiden silinir
            using (var connection = factory.CreateConnection())
            {
                //bağlantı hazırsa kanal oluşturalım
                using (var channel = connection.CreateModel())
                {
                    //şimdi kuyruk oluşturlaım
                    channel.QueueDeclare(queue: "task_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    //hello = kuyruk ismi.
                    //durable = false ise rabbitmq mesajı bellekte tutar. ve restart olduğunda uçar
                    //durable = true ise restart bile olsa diskte tutar birşey olmaz.
                    //exclusive = bu kuyruğa bir tane mi kanal bağlansın, başka  kanallarda bağlanabilsin mi ?
                    //autodelete = bir kuyrukta 20 tane mesaj var diyelim. kuyrukta ki mesaj bitince silinsinmi ?

                    string message = "Ömer Türkyılmaz Deneme Mesajı";

                    for (int i = 1; i < 11; i++)
                    {
                        var bodyByte = Encoding.UTF8.GetBytes($"{message}-{i}");
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;//Mesajıda sağlamaa aldık.

                        //mesajı gönderelim
                        channel.BasicPublish(exchange: "",
                                             routingKey: "task_queue",
                                             basicProperties: properties, 
                                             body: bodyByte);
                        //excahnge "" boş gönderdik. Default exchange oluyor.
                        //routingkey ile kuyruk ismimiz aynı olmalı.
                        //Default exhange kullanıyorsak route key kuyruk ismi ile aynı olması gerekiyor.

                        Console.WriteLine($"Mesajınız Gönderilmiştir : {message}-{i}");
                    }




                }


                Console.ReadLine();
            }
        }
    }
}
