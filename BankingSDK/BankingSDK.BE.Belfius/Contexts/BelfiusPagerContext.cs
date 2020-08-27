using BankingSDK.Common.Interfaces.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSDK.Base.BerlinGroup.Contexts
{
    public class BelfiusPagerContext : IPagerContext
    {
        [JsonProperty] private byte limit;
        [JsonProperty] private List<string> nextPageKeys;
        [JsonProperty] private int page;
        [JsonProperty] private bool lastPageReached;
        [JsonProperty] private uint totalPage;
        [JsonProperty] private uint? total;

        public BelfiusPagerContext() : this(10)
        {
        }

        public BelfiusPagerContext(byte limit)
        {
            this.limit = limit;
            page = 0;
            totalPage = 0;
            nextPageKeys = new List<string>();
            lastPageReached = false;
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
            // we didn't reach the last page
            if (!lastPageReached)
            {
                return false;
            }

            // the next page key is null
            return (page == nextPageKeys.Count - 1);
        }

        public void NextPage()
        {
            // we didn't start to crawl the list
            if (nextPageKeys.Count == 0)
            {
                return;
            }

            if (page < nextPageKeys.Count - 1)
            {
                page++;
            }
        }

        public void PreviousPage()
        {
            // we didn't start to crawl the list
            if (nextPageKeys.Count == 0)
            {
                return;
            }

            if (page > 0)
            {
                page--;
            }
        }

        public void SetLimit(byte limit)
        {
            if (limit < 1)
            {
                this.limit = 1;
            }
            else
            {
                if (limit > 99)
                {
                    this.limit = 99;
                }
                else
                {
                    this.limit = limit;
                }
            }

            page = 0;
            nextPageKeys = new List<string>();
            lastPageReached = false;
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
            // we didn't start to crawl the list
            if (nextPageKeys.Count == 0)
            {
                return;
            }

            if (page > nextPageKeys.Count)
            {
                this.page = (nextPageKeys.Count - 1);
            }

            this.page = (int) page - 1;
        }

        internal uint GetPage()
        {
            return (uint) page + 1;
        }

        public uint GetNextPage()
        {
            // we didn't start to crawl the list
            if (nextPageKeys.Count == 0)
            {
                return 0;
            }

            if (page == nextPageKeys.Count - 1)
            {
                return (uint) page;
            }

            return (uint) (page + 1);
        }

        public string GetRequestParams()
        {
            var next = "";

            if (page > 0)
            {
                next = "&next=" + nextPageKeys[page];
            }

            return $"?dateFrom=1980-01-01&bookingStatus=both&pagesize={limit}{next}";
            // return $"?dateFrom=1980-01-01&bookingStatus=both";
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void GoToPage(uint page)
        {
            if (page < 1)
            {
                this.page = 0;
            }
            else
            {
                if (page > nextPageKeys.Count)
                {
                    this.page = nextPageKeys.Count - 1;
                }
                else
                {
                    this.page = ((int) page) - 1;
                }
            }
        }

        public uint? RecordCount()
        {
            return null;
        }

        public uint? PageCount()
        {
            return (nextPageKeys.Count == 0 ? null : (uint?)nextPageKeys.Count);
        }

        public void AddNextPageKey(string key)
        {
            if (nextPageKeys.Count == 0)
            {
                nextPageKeys.Add("");
            }

            if (key == null)
            {
                lastPageReached = true;
            }
            else
            {
                if (!lastPageReached)
                {
                    nextPageKeys.Add(key);
                }
            }
        }
    }
}