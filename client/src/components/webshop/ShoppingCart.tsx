import React, {useState} from "react";
import {useCart} from "./CartContext.tsx"
import '../../Cart.css';

interface ShoppingCartProps {
    cartOpen: boolean;
    toggleCart: () => void;
}

const ShoppingCart: React.FC<ShoppingCartProps> = ({cartOpen, toggleCart}) => {
    const {cart, handleCheckout} = useCart();

    const calculateTotal = () => {
        return cart.reduce((total, item) => total + item.price * item.quantity, 0).toFixed(2);
        };

    return (
        <>
            {cartOpen && (
                <div className='cart-menu'>
                    <h2>Shopping Cart</h2>
                    {cart.length === 0 ? (
                        <p>Empty Cart</p>
                    ) : (
                        <div>
                            <ul>
                                {cart.map(item => (
                                    <li key={item.id}>
                                        {item.name} (Quantity: {item.quantity})
                                    </li>
                                ))}
                            </ul>
                            <h3>Total Price: {calculateTotal()}</h3>
                        </div>
                    )}
                    <button onClick={handleCheckout}>Checkout</button>
                </div>
            )}
        </>
    );
};
export default ShoppingCart;