using ElasticSearch7Template.Core;
using ElasticSearch7Template.Utility.Cache;
using ElasticSearch7Template.Utility.EnumClass;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Security.AccessControl;
using System.Threading;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    /// <summary>
    ///  rabbitmq帮助类基类
    /// </summary>
    public static class RabbitMQBaseHelper
    {
        /// <summary>
        /// 每次发送消息量
        /// </summary>
      //  public static ushort SendMessageCount = ;

        
        private const string CacheKey_MQMaxConnectionCount = "MQMaxConnectionCount";
        private readonly static ConcurrentQueue<IConnection> freeConnectionQueue;//空闲连接对象队列
        private readonly static ConcurrentDictionary<IConnection, bool> busyConnectionDic;//使用中（忙）连接对象集合
        private readonly static ConcurrentDictionary<IConnection, int> mqConnectionPoolUsingDicNew;//连接池使用率
        private readonly static Semaphore mqConnectionPoolSemaphore;
        private readonly static object freeConnLock = new object(), addConnLock = new object();
        private static int connCount = 0;
        private const int DefaultMaxConnectionCount = 5;//默认最大保持可用连接数
        private const int DefaultMaxConnectionUsingCount = 1000000;//默认最大连接可访问次数


        static RabbitMQBaseHelper()
        {
            freeConnectionQueue = new ConcurrentQueue<IConnection>();
            busyConnectionDic = new ConcurrentDictionary<IConnection, bool>();
            mqConnectionPoolUsingDicNew = new ConcurrentDictionary<IConnection, int>();//连接池使用率
            mqConnectionPoolSemaphore = new Semaphore(MaxConnectionCount, MaxConnectionCount);//信号量，控制同时并发可用线程数
        }

      

        private static int MaxConnectionCount
        {
            get
            {
                object maxConnectionCount = MemoryCacheHelper.Get(CacheKey_MQMaxConnectionCount);
                if (maxConnectionCount != null)
                {
                    return Convert.ToInt32(maxConnectionCount.ToString());
                }
                else
                {
                    int mqMaxConnectionCount = DefaultMaxConnectionCount;
                    var flag = MemoryCacheHelper.Set(CacheKey_MQMaxConnectionCount, mqMaxConnectionCount);
                    return mqMaxConnectionCount;
                }

            }
        }






        public static IConnection CreateMQConnectionInPoolNew()
        {

        SelectMQConnectionLine:

            mqConnectionPoolSemaphore.WaitOne();//当<MaxConnectionCount时，会直接进入，否则会等待直到空闲连接出现

            IConnection mqConnection = null;
            if (freeConnectionQueue.Count + busyConnectionDic.Count < MaxConnectionCount)//如果已有连接数小于最大可用连接数，则直接创建新连接
            {
                lock (addConnLock)
                {
                    if (freeConnectionQueue.Count + busyConnectionDic.Count < MaxConnectionCount)
                    {
                        mqConnection = CreateMQConnection();
                        busyConnectionDic[mqConnection] = true;//加入到忙连接集合中
                        mqConnectionPoolUsingDicNew[mqConnection] = 1;
                        return mqConnection;
                    }
                }
            }


            if (!freeConnectionQueue.TryDequeue(out mqConnection)) //如果没有可用空闲连接，则重新进入等待排队
            {
                goto SelectMQConnectionLine;
            }
            else if (mqConnectionPoolUsingDicNew[mqConnection] + 1 > DefaultMaxConnectionUsingCount || !mqConnection.IsOpen) //如果取到空闲连接，判断是否使用次数是否超过最大限制,超过则释放连接并重新创建
            {
                mqConnection.Close();
                mqConnection.Dispose();

                mqConnection = CreateMQConnection();
                mqConnectionPoolUsingDicNew[mqConnection] = 0;
            }

            busyConnectionDic[mqConnection] = true;//加入到忙连接集合中
            mqConnectionPoolUsingDicNew[mqConnection] = mqConnectionPoolUsingDicNew[mqConnection] + 1;//使用次数加1
            return mqConnection;
        }


        public static void ResetMQConnectionToFree(IConnection connection)
        {
            lock (freeConnLock)
            {
                bool result = false;
                if (busyConnectionDic.TryRemove(connection, out result)) //从忙队列中取出
                {
                }
                else
                {
                }

                if (freeConnectionQueue.Count + busyConnectionDic.Count > MaxConnectionCount)//如果因为高并发出现极少概率的>MaxConnectionCount，则直接释放该连接
                {
                    connection.Close();
                    connection.Dispose();
                }
                else
                {
                    freeConnectionQueue.Enqueue(connection);//加入到空闲队列，以便持续提供连接服务
                }

                mqConnectionPoolSemaphore.Release();//释放一个空闲连接信号
            }
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="hostName">服务器地址</param>
        /// <param name="userName">登录账号</param>
        /// <param name="passWord">登录密码</param>
        /// <returns></returns>
        private static ConnectionFactory CreateConnectionFactory()
        {

            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = RabbitMQConfig.RabbitMQUserName ?? "guest",
                Password = RabbitMQConfig.RabbitMQPassword ?? "guest",
                Port = RabbitMQConfig.RabbitMQPort,
                VirtualHost = RabbitMQConfig.RabbitMQVirtualHost,
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                NetworkRecoveryInterval = new TimeSpan(0, 0, 10)
            };
            return factory;
        }




        public static IConnection CreateMQConnection()
        {
            var factory = CreateConnectionFactory();
            var hosts = RabbitMQConfig.RabbitMQHostList;
            var connection = factory.CreateConnection(hosts);
            return connection;
        }

        public static string GetExchangeTypeString(ExchangeTypeEnum exchangeType)
        {
            string exchangeTypeString = "";
            switch (exchangeType)
            {
                case ExchangeTypeEnum.Fanout:
                    exchangeTypeString = ExchangeType.Fanout;
                    break;
                case ExchangeTypeEnum.Direct:
                    exchangeTypeString = ExchangeType.Direct;
                    break;
                case ExchangeTypeEnum.Topic:
                    exchangeTypeString = ExchangeType.Topic;
                    break;
                default:
                    exchangeTypeString = ExchangeType.Fanout;
                    break;
            }
            return exchangeTypeString;
        }
    }
}
