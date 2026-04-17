namespace Domain.Querys;

public class SqlQueries
{
    public const string GetProductsPaged = @"SELECT p.product_id as ProductId, p.product_name as ProductName, 
       c.category_name as CategoryName, p.unit_price as UnitPrice, p.supplier_id as SupplierId , p.quantity_per_unit as QuantityPerUnit,
       p.units_in_stock as UnitsInStock
       FROM products p
       INNER JOIN categories c ON p.category_id = c.category_id
       WHERE (@Search IS NULL OR p.search_vector @@ plainto_tsquery('spanish', @Search)) -- Cambio de to_tsvector a plainto_tsquery
         AND (@CategoryId IS NULL OR @CategoryId = 0 OR p.category_id = @CategoryId) -- Manejo de categoryId 0
         AND p.is_active = TRUE
       ORDER BY p.product_id
       LIMIT @PageSize OFFSET (@PageNumber - 1) * @PageSize";


    public const string GetProduct = @"SELECT product_id as ProductId,  product_name as ProductName,  supplier_id as SupplierId, 
             category_id as CategoryId, quantity_per_unit as QuantityPerUnit,   unit_price as UnitPrice, 
             units_in_stock as UnitsInStock,  reorder_level as ReorderLevel,  discontinued as Discontinued
            FROM products
            WHERE product_id = @ProductId AND is_active = TRUE;";

    public const string GetProductDetail = @"SELECT p.product_id as ProductId, p.product_name as ProductName, 
               c.category_name as CategoryName, c.description as CategoryDescription,
               c.picture as CategoryPicture, p.unit_price as UnitPrice, 
               p.units_in_stock as UnitsInStock
        FROM products p
        INNER JOIN categories c ON p.category_id = c.category_id
        WHERE p.product_id = @ProductId AND p.is_active = TRUE";

    public const string GetProductById = @"SELECT product_id as ProductId, product_name as ProductName,
            supplier_id as SupplierId, category_id as CategoryId, quantity_per_unit as QuantityPerUnit, unit_price as UnitPrice, 
            units_in_stock as UnitsInStock ,is_active as IsActive, created_at as CreatedAt, updated_at as UpdatedAt, 
            created_by as CreatedBy, updated_by as UpdatedBy ,
            search_vector, is_active, created_at, updated_at, created_by, updated_by FROM public.products 
            WHERE product_id =  @ProductId AND is_active = TRUE";

    public const string GetCategory = @"SELECT category_id as CategoryId, category_name as CategoryName, 
        description, picture FROM categories WHERE is_active = TRUE";

    public const string GetCategoryById = @"SELECT category_id as CategoryId, category_name as CategoryName,
        description, picture FROM categories WHERE category_id = @Id AND is_active = TRUE";

    public const string GetSuppliers = @"SELECT supplier_id as SupplierId, company_name as CompanyName, 
                               contact_name as ContactName, city, country 
                        FROM suppliers WHERE is_active = TRUE";

    public const string GetSupplierById = @"SELECT supplier_id as SupplierId, company_name as CompanyName, 
         contact_name as ContactName FROM suppliers WHERE supplier_id = @Id";

    public const string GetEmployees = @"SELECT employee_id as EmployeeId, first_name || ' ' || last_name as FullName, 
         username, title, is_active as IsActive  FROM employees WHERE is_active = TRUE";

    public const string GetEmployeesById = @"SELECT employee_id as EmployeeId, first_name || ' ' || last_name as FullName,
        username FROM employees WHERE employee_id = @Id";

    public const string GetOrderHistory = @"SELECT o.order_id as OrderId, o.customer_id as CustomerId, 
                           e.first_name || ' ' || e.last_name as EmployeeName, 
                           o.order_date as OrderDate,
                           SUM(od.unit_price * od.quantity) as Total FROM orders o
                           INNER JOIN employees e ON o.employee_id = e.employee_id
                           INNER JOIN order_details od ON o.order_id = od.order_id
                           GROUP BY o.order_id, o.customer_id, e.first_name, e.last_name, o.order_date
                           ORDER BY o.order_date DESC";


    public const string GetOrderById = @" SELECT o.order_id as OrderId, o.customer_id as CustomerId, 
            o.order_date as OrderDate, o.ship_address as ShipAddress,
            e.first_name || ' ' || e.last_name as EmployeeName,
            od.product_id as ProductId, p.product_name as ProductName, 
            od.unit_price as UnitPrice, od.quantity as Quantity, od.discount as Discount
            FROM orders o
            INNER JOIN employees e ON o.employee_id = e.employee_id
            INNER JOIN order_details od ON o.order_id = od.order_id
            INNER JOIN products p ON od.product_id = p.product_id
            WHERE o.order_id = @Id";

    public const string GetAllCustomers = @"SELECT customer_id as CustomerId, company_name as CompanyName, 
            contact_name as ContactName, city FROM customers WHERE is_active = TRUE";

    public const string GetCustomerById = @"SELECT customer_id as CustomerId, company_name as CompanyName, 
                     contact_name as ContactName,  city, phone 
                    FROM customers 
                    WHERE customer_id = @Id AND is_active = TRUE";

    public const string SaQuery = @"SELECT employee_id as EmployeeId, first_name || ' ' || last_name as FullName, 
                           username, password_hash as PasswordHash 
                    FROM employees WHERE username = @Username AND is_active = TRUE";
}
