using BankingSDK.Base.Ibanity.Enums;
using BankingSDK.Common.Interfaces.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Contexts
{
    public class IbanityPagerContext : IPagerContext
    {
        [JsonProperty]
        private byte limit;
        [JsonProperty]
        private string firstAfterId;
        [JsonProperty]
        private string previousPageId;
        [JsonProperty]
        private string nextPageId;
        [JsonProperty]
        private string requestParams;
        [JsonProperty]
        private PageDirection direction;

        public IbanityPagerContext() : this(10)
        {
        }

        public IbanityPagerContext(byte limit)
        {
            this.limit = limit;
            direction = PageDirection.FIRST;
        }

        public void FirstPage()
        {
            direction = PageDirection.FIRST;
            requestParams = "";
        }

        public bool IsFirstPage()
        {
            return string.IsNullOrEmpty(previousPageId) || (string.IsNullOrEmpty(nextPageId) && nextPageId == firstAfterId);
        }

        public bool IsLastPage()
        {
            return string.IsNullOrEmpty(nextPageId);
        }

        public void NextPage()
        {
            direction = PageDirection.NEXT;
            requestParams = $"&after={nextPageId ?? throw new Exception()}";
        }

        public void PreviousPage()
        {
            direction = PageDirection.PREVIOUS;
            requestParams = $"&before={previousPageId ?? throw new Exception()}";
        }

        public byte GetLimit()
        {
            return limit;
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

            firstAfterId = null;
            previousPageId = null;
            nextPageId = null;
            requestParams = null;
        }

        internal void SetBefore(string beforeId)
        {
            previousPageId = beforeId;
        }

        internal void SetAfter(string afterId)
        {
            if ((afterId != null) && (firstAfterId == null))
            {
                firstAfterId = afterId;
            }

            nextPageId = afterId;
        }

        internal string GetRequestParams()
        {
            return $"?limit={limit + (direction == PageDirection.PREVIOUS ? 0 : 1)}{requestParams}";
        }

        internal PageDirection GetDirection()
        {
            return direction;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void GoToPage(uint page)
        {
            throw new NotImplementedException();
        }

        public uint? RecordCount()
        {
            return null;
        }

        public uint? PageCount()
        {
            return null;
        }
    }
}
