import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { productService } from '../../../services/productService';
import '../../../styles/components.css';

const ProductFormPage = () => {
  const { id } = useParams();
  const { state } = useLocation();
  const navigate = useNavigate();
  const isEdit = Boolean(id);

  const [categories, setCategories] = useState(state?.categories || []);
  const [suppliers, setSuppliers] = useState(state?.suppliers || []);
  const [loadingProduct, setLoadingProduct] = useState(isEdit);
  const [submitLoading, setSubmitLoading] = useState(false);
  const [loadError, setLoadError] = useState('');

  const { register, handleSubmit, reset, formState: { errors } } = useForm({
    defaultValues: {
      productName: '',
      categoryId: '',
      supplierId: '',
      quantityPerUnit: '',
      unitPrice: '',
      unitsInStock: '',
      reorderLevel: '',
      discontinued: 'false'
    }
  });

  useEffect(() => {
    const loadCatalogs = async () => {
      try {
        const [categoryRes, supplierRes] = await Promise.all([
          productService.getCategories(),
          productService.getSuppliers()
        ]);

        if (categoryRes?.succeeded) setCategories(categoryRes.data || []);
        if (supplierRes?.succeeded) setSuppliers(supplierRes.data || []);
      } catch (error) {
        console.error('Error cargando catálogos', error);
        setLoadError('No se pudieron cargar las categorías y proveedores.');
      } 
    };

    loadCatalogs();
  }, []);

  useEffect(() => {
    const loadProduct = async () => {
      if (!isEdit) {
        setLoadingProduct(false);
        return;
      }

      let productData = state?.product;

      if (!productData) {
        try {
          const res = await productService.getById(parseInt(id, 10));
          if (res?.succeeded && res.data) {
            productData = res.data;
          } else {
            setLoadError('No se pudo cargar el producto para editar.');
            setLoadingProduct(false);
            return;
          }
        } catch (error) {
          console.error('Error al obtener el producto', error);
          setLoadError('No se pudo cargar el producto para editar.');
          setLoadingProduct(false);
          return;
        }
      }

      reset({
        productName: productData.productName || productData.productname || '',
        categoryId: String(productData.categoryId || productData.CategoryId || productData.categoryid || productData.CategoryID || ''),
        supplierId: String(productData.supplierId || productData.SupplierId || productData.supplierid || productData.SupplierID || ''),
        quantityPerUnit: productData.quantityPerUnit || productData.QuantityPerUnit || productData.quantityperunit || '',
        unitPrice: productData.unitPrice != null ? String(productData.unitPrice) : '',
        unitsInStock: productData.unitsInStock != null ? String(productData.unitsInStock) : '',
        reorderLevel: productData.reorderLevel != null ? String(productData.reorderLevel) : '',
        discontinued: String(productData.discontinued ?? false)
      });

      setLoadError('');
      setLoadingProduct(false);
    };

    loadProduct();
  }, [id, isEdit, state?.product, reset]);

  const onSubmit = async (data) => {
    setSubmitLoading(true);
    try {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      const payload = {
        productId: isEdit ? parseInt(id, 10) : 0,
        productName: data.productName.trim(),
        categoryId: parseInt(data.categoryId, 10),
        supplierId: parseInt(data.supplierId, 10),
        quantityPerUnit: data.quantityPerUnit.trim(),
        unitPrice: parseFloat(data.unitPrice),
        unitsInStock: parseInt(data.unitsInStock, 10),
        reorderLevel: parseInt(data.reorderLevel, 10),
        discontinued: data.discontinued === 'true',
        ...(isEdit
          ? { updatedBy: user?.employeeId || 0 }
          : { createdBy: user?.employeeId || 0 })
      };

      const res = isEdit ? await productService.update(payload) : await productService.create(payload);
      if (res?.succeeded) {
        navigate('/products');
      } else {
        setLoadError(res?.message || 'No se pudo guardar el producto.');
      }
    } catch (error) {
      console.error('Error al guardar el producto', error);
      setLoadError('Error inesperado al guardar el producto.');
    } finally {
      setSubmitLoading(false);
    }
  };

  return (
    <div className="product-form-page">
      <div className="product-form-page__header">
        <h2 className="product-form-page__title">{isEdit ? 'Editar Autoparte' : 'Nueva Autoparte'}</h2>
        <button type="button" className="product-form-page__back-btn" onClick={() => navigate('/products')}>
          ← Volver a Productos
        </button>
      </div>

      {loadError && <div className="product-form-page__error-message">{loadError}</div>}

      <form className="product-form-page__form" onSubmit={handleSubmit(onSubmit)}>
        <div className="product-form-page__field product-form-page__field--full-width">
          <label className="product-form-page__label">Nombre</label>
          <input
            className="product-form-page__input"
            {...register('productName', { required: 'El nombre es obligatorio' })}
          />
          {errors.productName && <span className="product-form-page__field-error">{errors.productName.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Categoría</label>
          <select
            className="product-form-page__select"
            {...register('categoryId', { required: 'La categoría es obligatoria' })}
          >
            <option value="">Seleccione...</option>
            {categories.map((c) => (
              <option key={c.categoryId} value={String(c.categoryId)}>{c.categoryName}</option>
            ))}
          </select>
          {errors.categoryId && <span className="product-form-page__field-error">{errors.categoryId.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Proveedor</label>
          <select
            className="product-form-page__select"
            {...register('supplierId', { required: 'El proveedor es obligatorio' })}
          >
            <option value="">Seleccione...</option>
            {suppliers.map((s) => (
              <option key={s.supplierId} value={String(s.supplierId)}>{s.companyName}</option>
            ))}
          </select>
          {errors.supplierId && <span className="product-form-page__field-error">{errors.supplierId.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Cantidad por Unidad</label>
          <input
            className="product-form-page__input"
            {...register('quantityPerUnit', { required: 'La cantidad por unidad es obligatoria' })}
          />
          {errors.quantityPerUnit && <span className="product-form-page__field-error">{errors.quantityPerUnit.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Precio Unitario</label>
          <input
            className="product-form-page__input"
            type="number"
            step="0.0001"
            {...register('unitPrice', { required: 'El precio unitario es obligatorio' })}
          />
          {errors.unitPrice && <span className="product-form-page__field-error">{errors.unitPrice.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Stock</label>
          <input
            className="product-form-page__input"
            type="number"
            {...register('unitsInStock', { required: 'El stock es obligatorio' })}
          />
          {errors.unitsInStock && <span className="product-form-page__field-error">{errors.unitsInStock.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Nivel Reorden</label>
          <input
            className="product-form-page__input"
            type="number"
            {...register('reorderLevel', { required: 'El nivel de reorden es obligatorio' })}
          />
          {errors.reorderLevel && <span className="product-form-page__field-error">{errors.reorderLevel.message}</span>}
        </div>

        <div className="product-form-page__field">
          <label className="product-form-page__label">Estado</label>
          <select
            className="product-form-page__select"
            {...register('discontinued', { required: 'El estado es obligatorio' })}
          >
            <option value="false">Activo</option>
            <option value="true">Discontinuado</option>
          </select>
          {errors.discontinued && <span className="product-form-page__field-error">{errors.discontinued.message}</span>}
        </div>

        <button
          className={`product-form-page__submit-btn ${submitLoading ? 'product-form-page__submit-btn--loading' : ''}`}
          type="submit"
          disabled={submitLoading}
        >
          {submitLoading ? 'Guardando...' : isEdit ? 'ACTUALIZAR DATOS' : 'GUARDAR PRODUCTO'}
        </button>
      </form>
    </div>
  );
};

export default ProductFormPage;