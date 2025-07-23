using BlogMagangementSystem.Common.Entities;
using MediatR;

namespace BlogMagangementSystem.Features.PostFeatures.UpdatePostFeature
{
    public class UpdatePostDto
    {
        private readonly IMediator _mediator;

        public UpdatePostDto()
        {
            
        }
        public UpdatePostDto(IMediator mediator)
        {
            _mediator = mediator;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<string> Comments { get; set; } = new HashSet<string>();
        public ICollection<string> Tags { get; set; } = new HashSet<string>();

        public async Task Run()
        {
          await  _mediator.Send(new UpdatePostByIdCommand(this));

        }

    }
}