using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Structures.RequestStructure;
using BlogMagangementSystem.Common.Structures.ResponseStructure;
using Microsoft.AspNetCore.Mvc;

namespace BlogMagangementSystem.Features.PostModule
{
    public class AddPostEndpoint :BaseEndpoint<AddPostRequestViewModel,AddPostResponseViewModel>
    {
        public AddPostEndpoint(BaseEndpointParameters<AddPostRequestViewModel> parameters ) :base(parameters){ }
        

    }
}
