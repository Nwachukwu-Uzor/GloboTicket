using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly IAsyncRepository<Category> _categoriesRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IAsyncRepository<Category> categoriesRepository, IMapper mapper)
        {
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createCategoryReponse = new CreateCategoryCommandResponse();

            var validator = new CreateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createCategoryReponse.Success = false;
                createCategoryReponse.ValidationErrors = new List<string>();
                foreach(var validationError in validationResult.Errors)
                {
                    createCategoryReponse.ValidationErrors.Add(validationError.ErrorMessage);
                }
            }

            if (createCategoryReponse.Success)
            {
                var category = new Category { Name = request.Name };
                category = await _categoriesRepository.AddAsync(category);
                createCategoryReponse.Category = _mapper.Map<CreateCategoryDto>(category);
            }

            return createCategoryReponse;
        }
    }
}
