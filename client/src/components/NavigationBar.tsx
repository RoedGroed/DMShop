import {useNavigate} from 'react-router-dom';
import '../Styles.css';
import {useState} from "react";
// @ts-ignore
export default function NavigationBar() {
    const navigate = useNavigate();
    const [menuOpen, setMenuOpen] = useState(false);

    function handleNavigate(route: string) {
        navigate(route);
        setMenuOpen(false);
    }

    function toggleMenu() {
        setMenuOpen(!menuOpen);
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
            </div>

            {/* The Burger Context */}
            {menuOpen && (
            <div className='context-menu'>
                <button className='context-button' onClick={() => handleNavigate('/products')}>Products</button>
                <button className='context-button' onClick={() => handleNavigate('/order')}>Orders</button>
            </div>
            )}

        </div>
    )
}