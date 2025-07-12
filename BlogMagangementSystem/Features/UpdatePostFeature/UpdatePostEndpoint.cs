using BlogMagangementSystem.Common.Structures.ResponseStructure;

namespace BlogMagangementSystem.Features.UpdatePostFeature
{
    public class UpdatePostEndpoint :BaseEndpoint<UpdatePostRequestViewModel,UpdatePostResponseViewModel>
    {
        public UpdatePostEndpoint(BaseEndpointParameters<UpdatePostRequestViewModel> parameters): base(parameters){}
    }
}
