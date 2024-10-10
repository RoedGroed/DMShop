import React, { useState, useEffect } from "react";
import { http } from "../../http";
import { OrderDetailsDto, OrderListDto } from "../../Api";
import OrderModal from "./OrderModal";
import toast from "react-hot-toast";

const CustomerOrderHistory = () => {
    const [orders, setOrders] = useState<OrderListDto[]>([]);
    const [selectedOrder, setSelectedOrder] = useState<OrderDetailsDto | null>(null);
    const [modalOpen, setModalOpen] = useState(false);

    useEffect(() => {
        http.api.orderGetRandomCustomerOrderHistory()
            .then((res) => {
                setOrders(res.data);
            })
            .catch((error) => {
                if (error.response) {
                    toast.error(error.response.data.error || "Failed to retrieve random customer orders.");
                } else {
                    toast.error("An unexpected error occurred.");
                }
            });
    }, []);


    const handleOnClickOrder = async (orderId) => {
        try {
            const res = await http.api.orderGetOrderById(orderId);
            setSelectedOrder(res.data);
            setModalOpen(true);
        } catch (error) {
            if (error.response) {
                toast.error(error.response.data.error || "Failed to fetch order details.");
            } else {
                toast.error("An unexpected error occurred.");
            }
        }
    }


    const handleCloseModal = () => {
        setModalOpen(false);
        setSelectedOrder(null);
    };

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center p-4 space-y-8">
            <h1 className="text-2xl font-bold text-center text-white">
                Order History
            </h1>
            <ul className="space-y-2 w-full max-w-5xl">
                {orders.map((order) => (
                    <li
                        key={order.id}
                        className="p-2 bg-gray-900 rounded-lg flex items-center text-white text-base cursor-pointer"
                        onClick={() => handleOnClickOrder(order.id)}
                    >
                        {/* Order Information */}
                        <p className="w-2/12">
                            <span className="font-bold">Order:</span> {order.id}
                        </p>
                        <p className="w-3/12">
                            <span className="font-bold">Customer:</span> {order.customerName || "N/A"}
                        </p>
                        <div className="w-3/12 text-sm space-y-0.5">
                            <p>Ordered: {new Date(order.orderDate).toLocaleString()}</p>
                            <p>Delivery: {order.deliveryDate}</p>
                        </div>
                        <p className="w-2/12">
                            <span className="font-bold">Total: $</span> {order.totalAmount.toFixed(2)}
                        </p>

                        {/* Status */}
                        <div className="w-2/12 flex items-center justify-end space-x-2">
                        <span
                            className={`font-bold px-2 py-1 rounded text-center ${
                                order.status === "delivered"
                                    ? "bg-green-500"
                                    : order.status === "processing"
                                        ? "bg-yellow-500"
                                        : order.status === "cancelled"
                                            ? "bg-red-500"
                                            : "bg-gray-500"
                            } text-white`}
                        >
                            {order.status}
                        </span>
                        </div>
                    </li>
                ))}
            </ul>

            {/* Modal to order details */}
            {modalOpen && selectedOrder && (
                <OrderModal order={selectedOrder} onClose={handleCloseModal}/>
            )}
        </div>
    );

};

export default CustomerOrderHistory;
