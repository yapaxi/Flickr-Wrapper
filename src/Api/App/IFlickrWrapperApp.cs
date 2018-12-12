using FlickrWrapper.Api.Models;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.App
{
    public interface IFlickrWrapperApp
    {
        Task<AppResponse<string>> GetImageDetails(string id);
        Task<AppResponse<string>> Search(FlickrSearchArguments searchArguments = null);
    }
}