using DiscountGrpc.Data;
using DiscountGrpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiscountGrpc.Services;


public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger): DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is null)
        {
            coupon = new Coupon{ProductName = "No Discount", Amount = 0, Description = "No Discount Desc"};
        }
        logger.LogInformation("Discount is retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Discount is created on product {coupon.ProductName} with amount {coupon.Amount}");
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Discount is updated on product {coupon.ProductName} with amount {coupon.Amount}");
        return coupon.Adapt<CouponModel>();
    }
    
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is not null)
        {
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Product {coupon.ProductName} is deleted with amount {coupon.Amount}");
            return new DeleteDiscountResponse { Success = true };
        }
        throw new RpcException(new Status(StatusCode.NotFound, "Discount not found for product " + request.ProductName));
    }
}