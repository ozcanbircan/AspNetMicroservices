﻿using AutoMapper;
using Discount.API.Repositories;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services {

	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase {
		private readonly IDiscountRepository _repository;
		private readonly ILogger<DiscountService> _logger;
		private readonly IMapper _mapper;

		public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper) {
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context) {
			var coupon = await _repository.GetDiscount(request.ProductName);
			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

			_logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

			return _mapper.Map<CouponModel>(coupon);
		}

		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context) {
			Coupon coupon = _mapper.Map<Coupon>(request.Coupon);

			await _repository.CreateDiscount(coupon);
			_logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

			return _mapper.Map<CouponModel>(coupon);
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context) {
			var coupon = _mapper.Map<Coupon>(request.Coupon);

			await _repository.UpdateDiscount(coupon);
			_logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

			return _mapper.Map<CouponModel>(coupon);
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context) {
			bool deleted = await _repository.DeleteDiscount(request.ProductName);
			return new DeleteDiscountResponse { Success = deleted };
		}
	}
}