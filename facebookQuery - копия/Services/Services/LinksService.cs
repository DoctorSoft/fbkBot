using System.Linq;
using System.Text.RegularExpressions;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Links;
using DataBase.QueriesAndCommands.Queries.Links;
using Services.ViewModels.LinksModels;

namespace Services.Services
{
    public class LinksService
    {
        public LinkList GetLinks()
        {
            var links = new GetLinksQueryHandler(new DataBaseContext()).Handle(new GetLinksQuery());

            return new LinkList
            {
                Links = links.Select(data => new Link
                {
                    Id = data.Id,
                    Name = data.Name
                }).ToList()
            };
        }

        public void AddNewLink(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var regex = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)");
            var result = regex.IsMatch(name);

            if (!result)
            {
                return;
            }

            new AddNewLinkCommandHandler(new DataBaseContext()).Handle(new AddNewLinkCommand
            {
                Name = name
            });
        }

        public void RemoveLink(long linkId)
        {
            new RemoveLinkCommandHandler(new DataBaseContext()).Handle(new RemoveLinkCommand
            {
                Id = linkId
            });
        }

        public void UpdateLink(long linkId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var regex = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)");
            var result = regex.IsMatch(name);


            if (!result)
            {
                return;
            }

            new UpdateLinkCommandHandler(new DataBaseContext()).Handle(new UpdateLinkCommand
            {
                Name = name,
                Id = linkId
            });
        }
    }
}
