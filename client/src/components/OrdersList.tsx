import React, { useEffect } from "react";
import { useAtom } from "jotai";
import { OrdersAtom } from "../atoms/OrdersAtom";
import { useInitializeData } from "../initializers/useInitializeOrders";
import { CogIcon } from '@heroicons/react/24/solid';

const OrdersList = () => {
    const [orders] = useAtom(OrdersAtom);

    useInitializeData();

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center justify-start pt-16">
            <ul className="space-y-2 w-3/4">
                {orders.map((order) => (
                    <li key={order.id} className="p-2 bg-gray-900 rounded-lg flex items-center text-white text-lg">

                        {/* Order Information */}
                        <p className="w-2/12">
                            <span className="font-bold">Order:</span> {order.id}
                        </p>
                        <p className="w-3/12">
                            <span className="font-bold">Customer:</span> {order.customerName || "N/A"}
                        </p>

                        <div className="w-3/12 text-sm space-y-0.5">
                            <p>Ordered: {order.orderDate ? new Date(order.orderDate).toLocaleDateString() : "N/A"}</p>
                            <p>Delivery: {order.deliveryDate ? new Date(order.deliveryDate).toLocaleDateString() : "N/A"}</p>
                        </div>

                        <p className="w-2/12">
                            <span className="font-bold">Total:</span> {order.totalAmount !== undefined ? order.totalAmount.toFixed(2) : "0.00"}
                        </p>

                        {/* Status and Settings */}
                        <div className="w-2/12 flex items-center justify-end space-x-2">
                            <span className={`font-bold px-4 py-2 rounded w-28 text-center ${
                                order.status === "delivered"
                                    ? "bg-green-500"
                                    : order.status === "processing"
                                        ? "bg-yellow-500"
                                        : order.status === "cancelled"
                                            ? "bg-red-500"
                                            : "bg-gray-500"
                            } text-white`}>
                                {order.status}
                            </span>
                            <button
                                className="flex items-center justify-center h-10 w-10"
                                onClick={() => alert('Der kommer til at ske seje ting her')}
                            >
                                <CogIcon className="w-max h-max text-white hover:text-customBlue"/>
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OrdersList;
