import React, { useEffect } from "react";
import { useAtom } from "jotai";
import { OrdersAtom } from "../atoms/OrdersAtom";
import { useInitializeData } from "../initializers/useInitializeOrders";
import { CogIcon } from '@heroicons/react/24/solid';

const OrdersList = () => {
    const [orders] = useAtom(OrdersAtom);

    useInitializeData();

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center justify-center">
            <ul className="space-y-3 w-full max-w-5xl">
                {orders.map((order) => (
                    <li key={order.id} className="p-1 bg-gray-900 rounded-lg flex justify-between items-center text-white">

                        {/* Order Information */}
                        <p className="flex-1">
                            <span className="font-bold">Order:</span> {order.id}
                        </p>
                        <p className="flex-1">
                            <span className="font-bold">Customer:</span> {order.customerName || "N/A"}
                        </p>

                        <div className="flex-1 text-xs space-y-0.5">
                            <p>Ordered: {order.orderDate ? new Date(order.orderDate).toLocaleDateString() : "N/A"}</p>
                            <p>Delivery: {order.deliveryDate ? new Date(order.deliveryDate).toLocaleDateString() : "N/A"}</p>
                        </div>
                        
                        <p className="flex-1">
                            <span className="font-bold">Total:</span> {order.totalAmount !== undefined ? order.totalAmount.toFixed(2) : "0.00"}
                        </p>
                        
                        <div className="flex items-center space-x-2">
                            <span className={`font-bold px-2 py-1 rounded ${
                                order.status === "delivered"
                                    ? "bg-green-500"
                                    : order.status === "processing"
                                        ? "bg-yellow-500"
                                        : "bg-gray-500"
                            } text-white`}>
                                {order.status}
                            </span>
                            <button
                                    className="flex items-center justify-center h-6 w-6 rounded-full border border-gray-300 p-0"
                                onClick={() => alert('Der kommer til at ske seje ting her')}
                            >
                                <CogIcon className="w-5 h-5 text-white hover:text-customBlue"/>
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OrdersList;
