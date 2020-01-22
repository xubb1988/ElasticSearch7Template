using System;

namespace ElasticSearch7Template.Core
{
    public partial class Snowflake
    {
        /// <summary>
        /// 根据id获取对应的时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DateTime GetDateTimeById(long id)
        {
            string str_id = Convert.ToString(id, 2);
            int len = str_id.Length;

            int sequenceStart = len < int.Parse(machineIdShift.ToString()) ? 0 : len - int.Parse(machineIdShift.ToString());
            int workerStart = len < int.Parse(datacenterIdShift.ToString()) ? 0 : len - int.Parse(datacenterIdShift.ToString());
            int timeStart = len < int.Parse(timestampLeftShift.ToString()) ? 0 : len - int.Parse(timestampLeftShift.ToString());
            String sequence = str_id.Substring(sequenceStart, len - sequenceStart);
            String workerId = sequenceStart == 0 ? "0" : str_id.Substring(workerStart, sequenceStart - workerStart);
            String dataCenterId = workerStart == 0 ? "0" : str_id.Substring(timeStart, workerStart - timeStart);
            String time = timeStart == 0 ? "0" : str_id.Substring(0, timeStart);
            int sequenceInt = Convert.ToInt32(sequence, 2);


            int workerIdInt = Convert.ToInt32(workerId, 2);

            int dataCenterIdInt = Convert.ToInt32(dataCenterId, 2);

            long diffTime = Convert.ToInt64(time, 2);
            long timeLong = diffTime + twepoch;
            var date = GetTime(timeLong);
            return date;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        private static DateTime GetTime(long timeStamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddMilliseconds(timeStamp);
            return newDateTime.ToLocalTime();
        }
    }
}
