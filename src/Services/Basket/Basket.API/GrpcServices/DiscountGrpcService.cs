using Discount.Grpc.Protos;
using System;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices {

	public class DiscountGrpcService {

		private readonly DiscountProtoService.DiscountProtoServiceClient _discountService;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountService) {
			_discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
		}

		public async Task<CouponModel> GetDiscount(string productName) {
			var discountRequest = new GetDiscountRequest { ProductName = productName };
			return await _discountService.GetDiscountAsync(discountRequest);
		}
	}
}