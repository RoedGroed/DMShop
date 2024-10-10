import React from 'react';

function OrderModal({ order, onClose }) {
    return (
        <div className="fixed inset-0 bg-gray-800 bg-opacity-75 flex items-center justify-center">
            <div className="bg-customBlue text-white rounded-lg p-6 w-2/4 relative">
                <button className="absolute top-2 right-2 p-2 text-white" 
                        onClick={onClose}>
                    Close
                </button>
                <h2 className="text-2xl font-bold mb-4">Order Details</h2>
                <div className="flex justify-between items-center mb-6">
                    
                    {/* Order Information */}
                    <div className="w-1/2">
                        <p><strong>Order ID:</strong> {order.id}</p>
                        <p><strong>Order Date:</strong> {new Date(order.orderDate).toLocaleString()}</p>
                        <p><strong>Delivery Date:</strong> {order.deliveryDate}</p>
                        <p><strong>Status:</strong> {order.status}</p>
                    </div>
                    
                    {/* Customer Information */}
                    <div className="w-1/2">
                        <p><strong>Name:</strong> {order.customerName}</p>
                        <p><strong>Address:</strong> {order.customerAddress}</p>
                        <p><strong>Phone:</strong> {order.customerPhone}</p>
                        <p><strong>Email:</strong> {order.customerEmail}</p>
                    </div>
                </div>
                
                {/* Entries List */}
                <div className="mb-6">
                    <h3 className="font-semibold">Order Entries</h3>
                    <ul>
                        {order.orderEntries.map(entry => (
                            <li key={entry.productName} className="p-2 bg-gray-900 rounded-lg mb-2 flex justify-between">
                                <span>{entry.productName}</span>
                                <span>{entry.quantity} x {entry.price?.toFixed(2)} = <strong>${entry.totalPrice?.toFixed(2)}</strong></span>
                            </li>
                        ))}
                    </ul>
                </div>
                
                {/* Total Amount */}
                <hr className="border-t border-gray-800 my-4" />
                <div className="font-bold text-right">
                    <p>Total: ${order.totalAmount?.toFixed(2)}</p>
                </div>
            </div>
        </div>
    );
}

export default OrderModal;
