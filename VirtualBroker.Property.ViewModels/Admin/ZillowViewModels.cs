using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.ViewModels.Admin
{
    public class AddApiRequest_ZillowViewModel : AddEntityViewModel<Guid, APIRequests_Zillow>, IAPIRequests_Zillow
    {
        private string apiKey = string.Empty;
        public string ApiKey 
        {
            get => apiKey;
            set => this.RaiseAndSetIfChanged(ref apiKey, value);
        }
        private string apiHost = string.Empty;
        public string ApiHost
        {
            get => apiHost;
            set => this.RaiseAndSetIfChanged(ref apiHost, value);
        }

        private string requestUri = string.Empty;
        public string RequestUri
        {
            get => requestUri;
            set => this.RaiseAndSetIfChanged(ref requestUri, value);
        }

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        private string code = string.Empty;
        public string Code
        {
            get => code;
            set => this.RaiseAndSetIfChanged(ref code, value);
        }
        public AddApiRequest_ZillowViewModel(IBusinessRepositoryFacade<APIRequests_Zillow, Guid> facade, ILogger<AddEntityViewModel<Guid, APIRequests_Zillow, IBusinessRepositoryFacade<APIRequests_Zillow, Guid>>> logger) : base(facade, logger)
        {
        }

        protected override Task<APIRequests_Zillow> ConstructEntity()
        {
            return Task.FromResult(new APIRequests_Zillow()
            {
                ApiKey = ApiKey,
                ApiHost = apiHost,
                RequestUri = requestUri,
                Name = name,
                Code = code,
            });
        }

        protected override Task Clear()
        {
            ApiKey = string.Empty;
            ApiHost = string.Empty;
            RequestUri = string.Empty;
            Name = string.Empty;
            Code = string.Empty;
            return Task.CompletedTask;
        }
    }
    public class ApiRequest_ZillowsViewModel : EntitiesDefaultViewModel<Guid, APIRequests_Zillow, ApiRequest_ZillowViewModel, AddApiRequest_ZillowViewModel>
    {
        public ApiRequest_ZillowsViewModel(AddApiRequest_ZillowViewModel addViewModel, IBusinessRepositoryFacade<APIRequests_Zillow, Guid> facade, ILogger<EntitiesViewModel<Guid, APIRequests_Zillow, ApiRequest_ZillowViewModel, IBusinessRepositoryFacade<APIRequests_Zillow, Guid>>> logger) : base(addViewModel, facade, logger)
        {
        }

        protected override Task<ApiRequest_ZillowViewModel> Construct(APIRequests_Zillow entity, CancellationToken token)
        {
            return Task.FromResult(new ApiRequest_ZillowViewModel(Logger, Facade, entity));
        }
        protected override Func<IQueryable<APIRequests_Zillow>, IOrderedQueryable<APIRequests_Zillow>>? OrderBy()
        {
            return e => e.OrderBy(x => x.Code);
        }
    }
    public class ApiRequest_ZillowViewModel : EntityViewModel<Guid, APIRequests_Zillow>, IAPIRequests_Zillow
    {
        private string apiKey = string.Empty;
        public string ApiKey
        {
            get => apiKey;
            set => this.RaiseAndSetIfChanged(ref apiKey, value);
        }
        private string apiHost = string.Empty;
        public string ApiHost
        {
            get => apiHost;
            set => this.RaiseAndSetIfChanged(ref apiHost, value);
        }

        private string requestUri = string.Empty;
        public string RequestUri
        {
            get => requestUri;
            set => this.RaiseAndSetIfChanged(ref requestUri, value);
        }

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        private string code = string.Empty;
        public string Code
        {
            get => code;
            set => this.RaiseAndSetIfChanged(ref code, value);
        }
        public ApiRequest_ZillowViewModel(ILogger logger, IBusinessRepositoryFacade<APIRequests_Zillow, Guid> facade, Guid id) : base(logger, facade, id)
        {
        }

        public ApiRequest_ZillowViewModel(ILogger logger, IBusinessRepositoryFacade<APIRequests_Zillow, Guid> facade, APIRequests_Zillow entity) : base(logger, facade, entity)
        {
        }

        protected override Task<APIRequests_Zillow> Populate()
        {
            return Task.FromResult(new APIRequests_Zillow()
            {
                Id = Id,
                ApiKey = ApiKey,
                ApiHost = apiHost,
                RequestUri = requestUri,
                Name = name,
                Code = code,
            });
        }

        protected override Task Read(APIRequests_Zillow entity)
        {
            Id = entity.Id;
            ApiHost = entity.ApiHost;
            ApiKey = entity.ApiKey;
            RequestUri = entity.RequestUri;
            Name = entity.Name;
            Code = entity.Code;
            return Task.CompletedTask;
        }
    }
}
