import React, {useState} from 'react';
import '../../ProductList.css';
import { ProductDto } from '../../Api.ts';

interface ProductProps{
    products: ProductDto[];
    addToCart: (product: ProductDto) => void;
}

const ProductsList: React.FC<ProductProps> = ({ products, addToCart }) => {
    const [tooltip, setTooltip] = useState<string | null>(null);
    const [tooltipPosition, setTooltipPosition] = useState<{ top: number; left: number } | null>(null);
    return (
        <div className="product-list">
            {products.map((product) => (
                <div key={product.id} className="product-item">
                    <img className="product-image"/>
                    <div className="product-name-container">
                        <div className="product-name">{product.name}</div>
                        <span
                            className="info-icon"
                            onMouseEnter={(e) => {
                                const propertyNames = product.properties?.map(p => p.propertyName).join(', ') || 'No properties available';
                                setTooltip(propertyNames);
                                const rect = e.currentTarget.getBoundingClientRect();
                                setTooltipPosition({
                                    top: rect.top + window.scrollY - 30,
                                    left: rect.left + rect.width / 2
                                });
                            }}
                            onMouseLeave={() => {
                                setTooltip(null);
                                setTooltipPosition(null);
                            }}
                        >
                            ℹ️
                        </span>
                    </div>
                    <div className="product-actions">
                        <button className="add-button" onClick={() => addToCart(product)}>Add to cart</button>
                        <div className="product-price">${product.price}</div>
                    </div>
                    {tooltip && tooltipPosition && (
                        <div
                            className="tooltip"
                            style={{
                                top: tooltipPosition.top,
                                left: tooltipPosition.left,
                            }}
                        >
                            {tooltip}
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
};
export default ProductsList;