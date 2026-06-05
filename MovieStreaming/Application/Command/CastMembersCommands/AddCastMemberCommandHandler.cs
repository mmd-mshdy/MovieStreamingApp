using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Command.CastMembersCommands
{
    public class AddCastMemberCommandHandler : IRequestHandler<AddCastMemberCommand, Result<CastMemberDto>>
    {
        private readonly ICastMemberRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCastMemberCommandHandler(ICastMemberRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CastMemberDto>> Handle(AddCastMemberCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            if (dto == null) Result.Failure<CastMemberDto>(new("CastMember.Null", "CAst member can not be null"));
            IEnumerable<CastMembers> check = await _repository.GetByFullNameAsync(dto.name, dto.familyName);
            if (!check.Any()) return Result.Failure<CastMemberDto>(new("CastMember.Existence", " Cast member Already Exists"));
            var member = _mapper.Map<CastMembers>(dto);
            await _repository.CreateAsync(member);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(dto);

        }
    }
}
