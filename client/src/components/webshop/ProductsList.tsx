import React from 'react';
import '../../ProductList.css';

interface ProductDto {
    id: number;
    name?: string;
}

const ProductsList: React.FC<{products: ProductDto[]; addToCart: (product: ProductDto) => void; }> = ({products, addToCart}) => {
    return (
        <div className="product-list">
            {products.map((product) => (
                <div key={product.id} className="product-item">
                    <img className="product-image"/>
                    <div className="product-name">{product.name}</div>
                    <button onClick={() => addToCart(product)}> add to cart</button>
                </div>
            ))}
        </div>
    );
};
export default ProductsList;