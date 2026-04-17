import api from '../api/axiosConfig';

export const productService = {
    create: async (productData) => {
    const response = await api.post('/Product/InsertProduct', productData);
    return response.data;
  },

  getSuppliers: async () => {
    const response = await api.get('/Supplier/GetSuppliers');
    return response.data;
  },

  // POST: /Product/GetProducts - Recibe { search, categoryId, pageNumber, pageSize }
  getPaged: async (filters) => {
    const response = await api.post('/Product/GetProducts', filters);
    return response.data;
  },

  // GET: /Category/GetCategories
  getCategories: async () => {
    const response = await api.get('/Category/GetCategories');
    return response.data;
  },

  getById: async (productId) => {
    const response = await api.post('/Product/GetProductById', { productId });
    return response.data;
  },

  // PUT: /Product/ProductUpdate - Actualiza un producto (incluye productId en el body)
  update: async (productData) => {
    // Enviamos al endpoint de Product (asegúrate que el DTO incluya el producto)
    const response = await api.put('/Product/ProductUpdate', productData);
    return response.data;
  },

  // PUT: /Product/Product - Envía el productId en el body para desactivación lógica
  updateDesactivación: async (productData) => {
    // Enviamos al endpoint de Product (asegúrate que el DTO incluya el productId)
    const response = await api.put('/Product/Product', productData );
    return response.data;
  },

  // POST: /Product/Product - Carga masiva aleatoria de productos
  bulkInsertProducts: async (bulkData) => {
    const response = await api.post('/Product/Product', bulkData);
    return response.data;
  }
};