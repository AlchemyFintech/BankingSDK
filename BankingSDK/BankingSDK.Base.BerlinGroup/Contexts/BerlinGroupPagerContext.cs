using BankingSDK.Common.Interfaces.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.BerlinGroup.Contexts
{
    public class BerlinGroupPagerContext : IPagerContext
    {
        [JsonProperty]
        private byte limit;
        [JsonProperty]
        private uint page;
        [JsonProperty]
        private uint nextPage;
        [JsonProperty]
        private uint? totalPage;
        [JsonProperty]
        private uint? total;

        public BerlinGroupPagerContext() : this(25)
        {
        }

        public BerlinGroupPagerContext(byte limit)
        {
            this.limit = limit;
            page = 0;
        }

        public void FirstPage()
        {
            page = 0;
        }

        public bool IsFirstPage()
        {
            return page == 0;
        }

        public bool IsLastPage()
        {
            if (totalPage == null)
            {
                return false;
            }
            
            return page == totalPage;
        }

        public void NextPage()
        {
            nextPage = page + 1;
        }

        public void PreviousPage()
        {
            if (page > 0)
            {
                nextPage = page - 1;
            }
            else
            {
                nextPage = 0;
            }
        }

        public void SetLimit(byte limit)
        {
            this.limit = 25;
            page = 0;
            nextPage = 0;
        }

        public byte GetLimit()
        {
            return limit;
        }

        public void SetPageTotal(uint pageTotal)
        {
            this.totalPage = pageTotal;
        }

        internal void SetTotal(uint total)
        {
            this.total = total;
        }

        public void SetPage(uint page)
        {
            this.page = page;
        }

        internal uint GetPage()
        {
            return page;
        }

        public uint GetNextPage()
        {
            return nextPage;
        }

        public string GetRequestParams()
        {
            DateTime dateFrom = DateTime.UtcNow.AddDays(-90);
            string query =
                $"?dateFrom={dateFrom:yyyy-MM-dd}&bookingStatus=both&page={nextPage}"; 
            return query;
            // return $"?dateFrom=1980-01-01&bookingStatus=both";
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void GoToPage(uint page)
        {
            nextPage = (page - 1);
        }

        public uint? RecordCount()
        {
            if (total != null)
            {
                return total;
            }

            if (totalPage != null)
            {
                return totalPage * limit + 1;
            }
            
            return null;
        }

        public uint? PageCount()
        {
            if (totalPage != null)
            {
                return totalPage + 1;
            }
            return null;
        }
    }
}
