import React, { useEffect } from "react";
import { useAtom } from "jotai";
import { OrdersAtom } from "../atoms/OrdersAtom";
import { useInitializeData } from "../initializers/useInitializeOrders";

const OrdersList = () => {
    const [orders] = useAtom(OrdersAtom);

    useInitializeData();

    // Log order data for at se, om customerName er tomt eller mangler
    useEffect(() => {
        console.log("Orders:", orders);
    }, [orders]);

    return (
        <div className="orders-list">
            {orders.length === 0 ? (
                <p>No orders found</p>
            ) : (
                <ul className="space-y-4">
                    {orders.map((order) => (
                        <li
                            key={order.id}
                            className="p-4 bg-gray-100 rounded-lg shadow flex justify-between items-center space-x-4"
                        >
                            <p className="flex-1">
                                <span className="font-bold">Order Number:</span> {order.id}
                            </p>
                            <p className="flex-1">
                                <span className="font-bold">Customer Name:</span> {order.customerName || "N/A"}
                            </p>
                            <p className="flex-1">
                                <span className={`font-bold px-2 py-1 rounded ${
                                    order.status === "delivered"
                                        ? "bg-green-500 text-white"
                                        : order.status === "processing"
                                            ? "bg-yellow-500 text-white"
                                            : "bg-gray-500 text-white"
                                }`}>
                                    {order.status}
                                </span>
                            </p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default OrdersList;
