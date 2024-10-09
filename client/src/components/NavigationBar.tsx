import {useNavigate} from 'react-router-dom';
import '../Styles.css';
import {useState} from "react";
import { http } from "../http";
import axios from "axios";
import {ProductDto} from "../Api.ts";


export default function NavigationBar() {
    const navigate = useNavigate();
    const [menuOpen, setMenuOpen] = useState(false);
    const [cartOpen, setCartOpen] = useState(false);

    function handleNavigate(route: string) {
        navigate(route);
        setMenuOpen(false);
    }

    function toggleMenu() {
        setMenuOpen(!menuOpen);
    }

    function toggleCart() {
        setCartOpen(!cartOpen);
    }

    return (
        <div className='navigationbar'>
            <div className='navbar-burger'>
                <button className='burger-icon' onClick={toggleMenu}>
                    <span aria-hidden="true"></span>
                </button>
            </div>

            <div className='navbar-logo'>
                <button className='logo' onClick={()=> handleNavigate('/')} />
            </div>
            {/* Buttons to different pages of the website */}
            <div className='navigation-center'>
                <button className='nav-buttons' onClick={() => handleNavigate("/webshop")}>Webstore</button>
                <button className='nav-buttons' onClick={() => handleNavigate("/contact")}>Contact</button>
                <button className='nav-buttons' onClick={() => handleNavigate("/about")}>About Us</button>
                {/* Shopping cart*/}
                <img
                    className='shoppingcart-image'
                    onClick={toggleCart}/>
            </div>

            {/* The Burger Context */}
            {menuOpen && (
            <div className='context-menu'>
                <button className='context-button' onClick={() => handleNavigate('/products')}>Products</button>
                <button className='context-button' onClick={() => handleNavigate('/order')}>Orders</button>
            </div>
            )}
            {/*Shopping cart menu*/}
            {cartOpen && (
                <div className='cart-menu'>
                    <h2>Shopping Cart</h2>
                    <button>Checkout</button>
                </div>
            )}

        </div>
    )
}