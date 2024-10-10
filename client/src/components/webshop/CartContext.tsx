// CartContext.tsx
import React, { createContext, useContext, useState } from 'react';
import { CartItem } from './Interface.tsx';
import {http} from "../../http.ts";

interface CartContextType {
    cart: CartItem[];
    addToCart: (product: any) => void;
    handleCheckout: () => void;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [cart, setCart] = useState<CartItem[]>([]);

    const addToCart = (product: any) => {
        setCart((prevCart) => {
            const existingItem = prevCart.find(item => item.id === product.id);
            if (existingItem) {
                return prevCart.map(item =>
                    item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
                );
            } else {
                return [...prevCart, { ...product, quantity: 1 }];
            }
        });
    };

    const handleCheckout = async () => {
        console.log("Cart before checkout:", cart);
        if (!cart || cart.length === 0) {
            return;
        }

        const orderEntries = cart.map(item => ({
            productId: item.id,
            quantity: item.quantity,
        }));
        console.log("Order Entries:", orderEntries);
        const createOrder = {
            customerId: 1,
            items: orderEntries,
            orderDate: new Date().toISOString(),
            deliveryDate: new Date(new Date().setDate(new Date().getDate() + 3)).toISOString(),
            status: "pending",
            orderEntries: orderEntries,
        };


        try {
            const response = await http.api.orderCreateOrder(createOrder);
            setCart([]);
        } catch (error) {
            console.error("Error creating order ", error);
        }
    };
    return (
        <CartContext.Provider value={{ cart, addToCart, handleCheckout }}>
            {children}
        </CartContext.Provider>
    );
};

export const useCart = (): CartContextType => {
    const context = useContext(CartContext);
    if (!context) {
        throw new Error('useCart must be used within a CartProvider');
    }
    return context;
};
