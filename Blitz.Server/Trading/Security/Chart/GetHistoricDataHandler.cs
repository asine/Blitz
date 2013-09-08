using System;
using System.Collections.Generic;
using System.Net;

using Blitz.Common.Trading.Security.Chart;
using Blitz.Server.Core;

using Common.Logging;

namespace Blitz.Server.Trading.Security.Chart
{
    public class GetHistoricDataHandler : Handler<GetHistoricDataRequest, GetHistoricDataResponse>
    {
        public GetHistoricDataHandler(ILog log)
            : base(log)
        {
        }

        protected override GetHistoricDataResponse Execute(GetHistoricDataRequest request)
        {
            // http://www.jarloo.com/get-historical-stock-data/

            var results = new List<HistoricalDataDto>();

            using (var web = new WebClient())
            {
                var url = "http://ichart.finance.yahoo.com/table.csv?s=" + request.Ticker + "&d=" +
                          request.To.AddMonths(-1).Month + "&e=" + request.To.Day + "&f=" + request.To.Year + "&g=d&a=" +
                          request.From.AddMonths(-1).Month + "&b=" + request.From.Day + "&c=" + request.From.Year +
                          "&ignore=.csv";

                Log.Debug(string.Format("URL - {0}", url));

                var data = web.DownloadString(url);

                data = data.Replace("r", "");

                var rows = data.Split('\n');

                //First row is headers so Ignore it
                for (var i = 1; i < rows.Length; i++)
                {
                    if (rows[i].Replace("n", "").Trim() == "") continue;

                    var cols = rows[i].Split(',');

                    var historicalDataDto = new HistoricalDataDto
                    {
                        Date = Convert.ToDateTime(cols[0]),
                        Open = Convert.ToDouble(cols[1]),
                        High = Convert.ToDouble(cols[2]),
                        Low = Convert.ToDouble(cols[3]),
                        Close = Convert.ToDouble(cols[4]),
                        Volume = Convert.ToDouble(cols[5]),
                        AdjClose = Convert.ToDouble(cols[6])
                    };

                    results.Add(historicalDataDto);
                }
            }


            var response = CreateTypedResponse();

            response.Results = results;

            return response;
        }
    }
}