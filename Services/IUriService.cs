using PullDataApi.Models;
using System;

namespace PullDataApi.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
