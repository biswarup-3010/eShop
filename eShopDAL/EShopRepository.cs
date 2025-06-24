using System;
using System.Data;
using eShopDAL.Data;
using eShopDAL.DTOs;
using eShopDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace eShopDAL
{
	public class EShopRepository
	{
        #region Signature
        private EShopContext context;
        private readonly string _connectionString;

        public EShopRepository(EShopContext eShopContext, IConfiguration configuration)
        {
            this.context = eShopContext;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #endregion

        #region authentication

        #region login
        public (string Status, UserDto? User) LoginUserUsingSP(string email, string password)
        {
            string status = "Unknown";
            UserDto? user = null;

            using (var conn = new MySqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                using (var cmd = new MySqlCommand("UserLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Email", email);
                    cmd.Parameters.AddWithValue("@p_Password", password);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = reader["Status"].ToString();

                            if (status == "Success")
                            {
                                user = new UserDto
                                {
                                    UserId = Convert.ToInt32(reader["UserID"]),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    RoleId = Convert.ToInt32(reader["RoleID"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                                };
                            }
                        }
                    }
                }
            }

            return (status, user);
        }

        #endregion

        #region forget password
        public string ForgotPassword(string email, string newPassword)
        {
            string status = "";

            // Encrypt or hash password here if necessary
            string hashedPassword = newPassword; // Replace with actual hash logic in real use

            using (MySqlConnection conn = new MySqlConnection(this.context.Database.GetDbConnection().ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("ForgotPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.Add(new MySqlParameter("@p_Email", MySqlDbType.VarChar) { Value = email });
                    cmd.Parameters.Add(new MySqlParameter("@p_NewPassword", MySqlDbType.VarChar) { Value = hashedPassword });

                    // Output parameter
                    var statusParam = new MySqlParameter("@p_Status", MySqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(statusParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    status = statusParam.Value?.ToString() ?? "Unknown";
                }
            }

            return status;
        }

        #endregion

        #endregion

        #region User

        #region Register user
        public string RegisterUser(RegisterDto dto)
        {
            // Check if user already exists
            bool userExists = context.Users.Any(u => u.Email == dto.Email);
            if (userExists)
            {
                return "User Already Exists";
            }

            // Check if Role ID is valid
            bool roleExists = context.Roles.Any(r => r.RoleId == dto.RoleId);
            if (!roleExists)
            {
                return "Invalid Role ID";
            }

            string hashedPassword = dto.Password; // Add real hash logic

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Phone = dto.Phone,
                Address = dto.Address,
                RoleId = dto.RoleId,
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            return context.SaveChanges() > 0 ? "Success" : "Failed to Register";
        }



        #endregion

        #region Get All Users
        public List<UserDto> GetAllUsers()
        {
            return this.context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt
                })
                .ToList();
        }
        #endregion

        #region Get User
        public UserDto GetUser(int id)
        {
            return this.context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefault();
        }
        #endregion

        #region update user
        public string UpdateUser(UserDto dto)
        {
            string status = "Unknown";

            using (var conn = new MySqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                using (var cmd = new MySqlCommand("UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_UserId", dto.UserId);
                    cmd.Parameters.AddWithValue("@p_FullName", dto.FullName ?? "");
                    cmd.Parameters.AddWithValue("@p_Email", dto.Email ?? "");
                    cmd.Parameters.AddWithValue("@p_Phone", dto.Phone ?? "");
                    cmd.Parameters.AddWithValue("@p_Address", dto.Address ?? "");
                    cmd.Parameters.AddWithValue("@p_RoleId", dto.RoleId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_CreatedAt", dto.CreatedAt ?? DateTime.Now);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = reader["Status"]?.ToString() ?? "Unknown";
                        }
                    }
                }
            }

            return status;
        }


        #endregion

        #region delete user
        public bool DeleteUser(int userId)
        {
            // Find the user
            var user = context.Users
                .Include(u => u.Carts)
                .Include(u => u.Orders)
                .Include(u => u.Reviews)
                .Include(u => u.Wishlists)
                .Include(u => u.UserInterests)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return false;

            // Remove dependent entities first
            context.Carts.RemoveRange(user.Carts);
            context.Orders.RemoveRange(user.Orders);
            context.Reviews.RemoveRange(user.Reviews);
            context.Wishlists.RemoveRange(user.Wishlists);
            context.UserInterests.RemoveRange(user.UserInterests);

            // Finally remove the user
            context.Users.Remove(user);

            return context.SaveChanges() > 0;
        }

        #endregion

        #endregion

        #region Product Management

        #region GetAllProducts
        public List<ProductDto> GetAllProducts()
        {
            return this.context.Products
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId
                })
                .ToList();
        }
        #endregion

        #region Product by Id
        public ProductDto GetProductById(int id)
        {
            return this.context.Products.Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId
                })
                .FirstOrDefault();
        }
        #endregion

        #region Add new Product
        public bool AddProduct(ProductDto dto)
        {
            // Check if category exists
            bool categoryExists = this.context.Categories.Any(c => c.CategoryId == dto.CategoryId);
            if (!categoryExists)
            {
                throw new InvalidOperationException("Invalid Category ID");
            }

            // Optional: check for duplicate product
            bool exists = this.context.Products.Any(p => p.ProductName.ToLower() == dto.ProductName.ToLower());
            if (exists)
            {
                return false;
            }

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock ?? 1,
                CategoryId = dto.CategoryId
            };

            this.context.Products.Add(product);
            return this.context.SaveChanges() > 0;
        }


        #endregion

        #region update product
        public string UpdateProductUsingSP(ProductDto dto)
        {
            string status = "";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("UpdateProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_ProductId", dto.ProductId);
                    cmd.Parameters.AddWithValue("@p_ProductName", dto.ProductName ?? "");
                    cmd.Parameters.AddWithValue("@p_Description", dto.Description ?? "");
                    cmd.Parameters.AddWithValue("@p_Price", dto.Price);
                    cmd.Parameters.AddWithValue("@p_Stock", dto.Stock ?? 0);
                    cmd.Parameters.AddWithValue("@p_CategoryId", dto.CategoryId ?? (object)DBNull.Value);

                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = reader["Status"].ToString();
                        }
                    }
                }
            }

            return status;
        }

        #endregion

        #region Delete product
        public bool DeleteProduct(int productId)
        {
            // First, find the product
            var product = this.context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
                return false;

            // Remove dependent entities (in order)
            var cartItems = this.context.Carts.Where(c => c.ProductId == productId).ToList();
            var wishlistItems = this.context.Wishlists.Where(w => w.ProductId == productId).ToList();
            var orderDetails = this.context.OrderDetails.Where(o => o.ProductId == productId).ToList();
            var reviews = this.context.Reviews.Where(r => r.ProductId == productId).ToList();
            var images = this.context.ProductImages.Where(i => i.ProductId == productId).ToList();

            this.context.Carts.RemoveRange(cartItems);
            this.context.Wishlists.RemoveRange(wishlistItems);
            this.context.OrderDetails.RemoveRange(orderDetails);
            this.context.Reviews.RemoveRange(reviews);
            this.context.ProductImages.RemoveRange(images);

            // Finally, remove the product
            this.context.Products.Remove(product);

            return this.context.SaveChanges() > 0;
        }

        #endregion

        #endregion

        #region Category Management

        #region Get all categories
        public List<CategoryDto> GetAllCategories()
        {
            return context.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList();
        }
        #endregion

        #region get category by id
        public CategoryDto? GetCategoryById(int categoryId)
        {
            return context.Categories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefault();
        }

        #endregion

        #region Add a new category
        public bool AddCategory(CategoryDto dto)
        {
            var exists = context.Categories.Any(c => c.CategoryName.ToLower() == dto.CategoryName.ToLower());
            if (exists) return false;

            var category = new Category
            {
                CategoryName = dto.CategoryName
            };

            context.Categories.Add(category);
            return context.SaveChanges() > 0;
        }
        #endregion

        #region Update existing category
        public bool UpdateCategory(CategoryDto dto)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == dto.CategoryId);
            if (category == null) return false;

            category.CategoryName = dto.CategoryName;
            return context.SaveChanges() > 0;
        }
        #endregion

        #region Delete a category
        public bool DeleteCategory(int categoryId)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null) return false;

            context.Categories.Remove(category);
            return context.SaveChanges() > 0;
        }
        #endregion

        #endregion

        #region Cart

        #region Get cart
        public List<UserCartDto> GetCartByUserId(int userId)
        {
            var cartItems = context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new UserCartDto
                {
                    CartId = c.CartId,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductName,
                    Quantity = c.Quantity,
                    Price = c.Product.Price,
                    AddedAt = c.AddedAt
                })
                .ToList();

            return cartItems;
        }

        #endregion

        #region add to cart
        public string AddToCart(int userId, int productId, int quantity)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
                return "Product Not Found";

            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return "User Not Found";

            var existingCart = context.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCart != null)
            {
                existingCart.Quantity = (existingCart.Quantity ?? 0) + quantity;
                existingCart.AddedAt = DateTime.Now;
            }
            else
            {
                context.Carts.Add(new Models.Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.Now
                });
            }

            context.SaveChanges();
            return "Item Added to Cart";
        }

        #endregion

        #region remove item from cart
        public bool RemoveFromCart(int cartId)
        {
            var cartItem = context.Carts.FirstOrDefault(c => c.CartId == cartId);

            if (cartItem == null)
                return false;

            context.Carts.Remove(cartItem);
            return context.SaveChanges() > 0;
        }

        #endregion

        #endregion

        #region User Interest

        #region get user interest
        public List<UserInterestDto> GetUserInterests(int userId)
        {
            return context.UserInterests
                .Where(ui => ui.UserId == userId)
                .Select(ui => new UserInterestDto
                {
                    InterestId = ui.InterestId,
                    UserId = ui.UserId,
                    CategoryId = ui.CategoryId,
                    InterestedAt = ui.InterestedAt
                }).ToList();
        }

        #endregion

        #region add user interest
        public bool AddUserInterest(UserInterestDto dto)
        {
            var interest = new UserInterest
            {
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                InterestedAt = dto.InterestedAt ?? DateTime.UtcNow
            };

            context.UserInterests.Add(interest);
            return context.SaveChanges() > 0;
        }

        #endregion

        #region update user interest
        public bool UpdateUserInterest(UserInterestDto dto)
        {
            var existing = context.UserInterests.FirstOrDefault(i => i.InterestId == dto.InterestId);
            if (existing == null) return false;

            existing.CategoryId = dto.CategoryId;
            existing.InterestedAt = dto.InterestedAt ?? DateTime.UtcNow;

            return context.SaveChanges() > 0;
        }

        #endregion

        #region delete user interest
        public bool DeleteUserInterest(int interestId)
        {
            var interest = context.UserInterests.FirstOrDefault(i => i.InterestId == interestId);
            if (interest == null) return false;

            context.UserInterests.Remove(interest);
            return context.SaveChanges() > 0;
        }

        #endregion

        #endregion

        #region Order Management

        #region Get All Orders
        public List<OrderDto> GetAllOrders()
        {
            return this.context.Orders
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .ToList();
        }
        #endregion

        #region Get Order By Id
        public OrderDto GetOrderById(int id)
        {
            return this.context.Orders
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .FirstOrDefault();
        }
        #endregion

        #region Add Order
        public bool AddOrder(OrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = dto.OrderDate ?? DateTime.Now,
                Status = dto.Status ?? "Pending",
                TotalAmount = dto.TotalAmount ?? 0
            };

            this.context.Orders.Add(order);
            return this.context.SaveChanges() > 0;
        }
        #endregion

        #region Update Order
        public bool UpdateOrder(OrderDto dto)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.OrderId == dto.OrderId);

            if (order == null)
                return false;

            order.UserId = dto.UserId;
            order.OrderDate = dto.OrderDate;
            order.Status = dto.Status;
            order.TotalAmount = dto.TotalAmount;

            return this.context.SaveChanges() > 0;
        }
        #endregion

        #region Delete Order
        public bool DeleteOrder(int id)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return false;

            this.context.Orders.Remove(order);
            return this.context.SaveChanges() > 0;
        }
        #endregion

        #region Get Orders by UserId
        public List<OrderDto> GetOrdersByUser(int userId)
        {
            return this.context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .ToList();
        }
        #endregion

        #region Get Order Details by OrderId
        public List<OrderDetailDto> GetOrderDetailsByOrderId(int orderId)
        {
            return this.context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    PriceAtPurchase = od.PriceAtPurchase,
                    ProductName = od.Product != null ? od.Product.ProductName : null
                })
                .ToList();
        }
        #endregion

        #endregion

        #region Product Image Management

        #region Get product images by ProductId
        public List<ProductImageDto> GetImagesByProductId(int productId)
        {
            return this.context.ProductImages
                .Where(img => img.ProductId == productId)
                .Select(img => new ProductImageDto
                {
                    ImageId = img.ImageId,
                    ProductId = img.ProductId,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary
                }).ToList();
        }
        #endregion

        #region Add product image
        public bool AddProductImage(ProductImageDto dto)
        {
            var image = new ProductImage
            {
                ProductId = dto.ProductId,
                ImageUrl = dto.ImageUrl,
                IsPrimary = dto.IsPrimary ?? false
            };

            this.context.ProductImages.Add(image);
            return this.context.SaveChanges() > 0;
        }
        #endregion

        #region Update product image
        public bool UpdateProductImage(ProductImageDto dto)
        {
            var existing = this.context.ProductImages.FirstOrDefault(i => i.ImageId == dto.ImageId);
            if (existing == null) return false;

            existing.ProductId = dto.ProductId;
            existing.ImageUrl = dto.ImageUrl;
            existing.IsPrimary = dto.IsPrimary;

            return this.context.SaveChanges() > 0;
        }
        #endregion

        #region Delete product image
        public bool DeleteProductImage(int imageId)
        {
            var image = this.context.ProductImages.FirstOrDefault(i => i.ImageId == imageId);
            if (image == null) return false;

            this.context.ProductImages.Remove(image);
            return this.context.SaveChanges() > 0;
        }
        #endregion

        #endregion

        #region Purchase Management

        #region Get all purchases
        public List<PurchaseDto> GetAllPurchases()
        {
            return this.context.Purchases.Select(p => new PurchaseDto
            {
                PurchaseId = p.PurchaseId,
                OrderId = p.OrderId,
                PurchaseDate = p.PurchaseDate,
                PaymentMethod = p.PaymentMethod,
                PaymentStatus = p.PaymentStatus
            }).ToList();
        }
        #endregion

        #region Get purchase by ID
        public PurchaseDto? GetPurchaseById(int id)
        {
            var p = this.context.Purchases.FirstOrDefault(p => p.PurchaseId == id);
            if (p == null) return null;

            return new PurchaseDto
            {
                PurchaseId = p.PurchaseId,
                OrderId = p.OrderId,
                PurchaseDate = p.PurchaseDate,
                PaymentMethod = p.PaymentMethod,
                PaymentStatus = p.PaymentStatus
            };
        }
        #endregion

        #region Add new purchase
        public bool AddPurchase(PurchaseDto dto)
        {
            var purchase = new Purchase
            {
                OrderId = dto.OrderId,
                PurchaseDate = dto.PurchaseDate ?? DateTime.Now,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus
            };

            this.context.Purchases.Add(purchase);
            return this.context.SaveChanges() > 0;
        }
        #endregion

        #endregion

        #region Review Management

        public List<ReviewDto> GetAllReviews()
        {
            return this.context.Reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                ProductId = r.ProductId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            }).ToList();
        }

        public ReviewDto? GetReviewByUser(int userId)
        {
            var r = this.context.Reviews.FirstOrDefault(r => r.UserId == userId);
            if (r == null) return null;

            return new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                ProductId = r.ProductId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            };
        }

        public List<ReviewDto> GetReviewForProduct(int productId)
        {
            return this.context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    UserId = r.UserId,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                })
                .ToList();
        }


        public bool AddReview(ReviewDto dto)
        {
            var review = new Review
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = dto.ReviewDate ?? DateTime.Now
            };

            this.context.Reviews.Add(review);
            return this.context.SaveChanges() > 0;
        }

        public bool UpdateReview(ReviewDto dto)
        {
            var review = this.context.Reviews.FirstOrDefault(r => r.ReviewId == dto.ReviewId);
            if (review == null) return false;

            review.UserId = dto.UserId;
            review.ProductId = dto.ProductId;
            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.ReviewDate = dto.ReviewDate;

            return this.context.SaveChanges() > 0;
        }

        public bool DeleteReview(int id)
        {
            var review = this.context.Reviews.FirstOrDefault(r => r.ReviewId == id);
            if (review == null) return false;

            this.context.Reviews.Remove(review);
            return this.context.SaveChanges() > 0;
        }

        #endregion

        #region role
        public RoleDto? GetRoleById(int id)
        {
            var role = this.context.Roles.FirstOrDefault(r => r.RoleId == id);
            if (role == null) return null;

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        #endregion

        #region Wishlist Management

        public List<WishlistDto> GetUserWishlist(int userId)
        {
            return this.context.Wishlists
                .Where(w => w.UserId == userId)
                .Select(w => new WishlistDto
                {
                    WishlistId = w.WishlistId,
                    UserId = w.UserId,
                    ProductId = w.ProductId,
                    AddedAt = w.AddedAt
                }).ToList();
        }

        public bool AddToWishlist(WishlistDto dto)
        {
            var exists = this.context.Wishlists
                .Any(w => w.UserId == dto.UserId && w.ProductId == dto.ProductId);

            if (exists) return false;

            var wishlist = new Wishlist
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                AddedAt = dto.AddedAt ?? DateTime.Now
            };

            this.context.Wishlists.Add(wishlist);
            return this.context.SaveChanges() > 0;
        }

        public bool MoveWishlistItemToCart(int wishlistId)
        {
            var item = this.context.Wishlists.FirstOrDefault(w => w.WishlistId == wishlistId);
            if (item == null || item.UserId == null || item.ProductId == null)
                return false;

            var cart = new Cart
            {
                UserId = item.UserId,
                ProductId = item.ProductId,
                Quantity = 1,
                AddedAt = DateTime.Now
            };

            this.context.Carts.Add(cart);
            this.context.Wishlists.Remove(item);

            return this.context.SaveChanges() > 0;
        }

        public bool RemoveFromWishlist(int wishlistId)
        {
            var item = this.context.Wishlists.FirstOrDefault(w => w.WishlistId == wishlistId);
            if (item == null) return false;

            this.context.Wishlists.Remove(item);
            return this.context.SaveChanges() > 0;
        }

        #endregion

    }
}

