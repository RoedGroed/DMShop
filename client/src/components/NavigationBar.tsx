import {useNavigate} from 'react-router-dom';
import '../Styles.css';
import {useState} from "react";
import { useAtom } from 'jotai';
import { isAdminAtom } from '../atoms/IsAdminAtom';

// @ts-ignore
export default function NavigationBar() {
    const navigate = useNavigate();
    const [menuOpen, setMenuOpen] = useState(false);
    const [isAdmin, setIsAdmin] = useAtom(isAdminAtom);

    function handleNavigate(route: string) {
        navigate(route);
        setMenuOpen(false);
    }

    function toggleMenu() {
        setMenuOpen(!menuOpen);
    }
    
    const handleRoleToggle = () => {
        setIsAdmin(!isAdmin);
    };

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
                    {/* Toggle Button */}
                    <label className="flex cursor-pointer">
                        <span className="mr-1 text-white">Customer</span>
                        <div className="relative">
                            <input type="checkbox" checked={isAdmin} onChange={handleRoleToggle} className="sr-only" />
                            <div className="block bg-gray-600 w-10 h-6 rounded-full"></div>
                            <div className={`absolute left-1 top-1 w-4 h-4 rounded-full transition transform ${isAdmin ? 'translate-x-full bg-blue-500' : 'bg-green-500'}`}></div>
                        </div>
                        <span className="ml-1 text-white">Admin</span>
                    </label>
                    {isAdmin ? (
                        <>
                            <button className='context-button' onClick={() => handleNavigate('/products')}>Products</button>
                            <button className='context-button' onClick={() => handleNavigate('/order')}>Orders</button>
                        </>
                    ) : (
                        <button className='context-button' onClick={() => handleNavigate('/myorders')}>My Orders</button>
                    )}
                </div>
            )}

        </div>
    )
}