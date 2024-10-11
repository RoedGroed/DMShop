import React, { useState, useEffect } from "react";
import { useAtom } from "jotai";
import { OrdersAtom } from "../../atoms/OrdersAtom";
import { http } from "../../http";
import { OrderDetailsDto } from "../../Api";
import OrderModal from "./OrderModal";
import StatusChanger from "./StatusChanger";
import toast from "react-hot-toast";

const OrdersList = () => {
    const [orders, setOrders] = useAtom(OrdersAtom);
    const [page, setPage] = useState(0);
    const [pageSize] = useState(10);
    const [selectedOrder, setSelectedOrder] = useState<OrderDetailsDto | null>(null);
    const [modalOpen, setModalOpen] = useState(false);

    useEffect(() => {
        http.api.orderGetOrdersForList({ limit: pageSize, startAt: page * pageSize })
            .then((res) => {
                setOrders(res.data);
            })
            .catch((error) => {
                if (error.response) {
                    toast.error(error.response.data.message || "Failed to retrieve orders.");
                } else {
                    toast.error("An error occurred while retrieving the orders.");
                }
            });
    }, [page, pageSize]);


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
    }

    const handleStatusChange = () => {
        http.api.orderGetOrdersForList({ limit: pageSize, startAt: page * pageSize })
            .then((res) => {
                setOrders(res.data);
            })
            .catch((error) => {
                if (error.response) {
                    toast.error(error.response.data.message || "Failed to update order list.");
                } else {
                    toast.error("An error occurred while updating the order list.");
                }
            });
    };
    
    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center justify-start pt-16">
            <ul className="space-y-2 w-3/4">
                {orders.map((order) => (
                    <li key={order.id} className="p-2 bg-gray-900 rounded-lg flex items-center text-white text-lg cursor-pointer"
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
                            <span className="font-bold">Total: $</span>{order.totalAmount.toFixed(2)} 
                        </p>

                        {/* Status */}
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
                            <div
                                className="w-1/6 justify-end space-x-2"
                                onClick={(e) => e.stopPropagation()}>
                                <StatusChanger
                                    orderId={order.id}
                                    currentStatus={order.status}
                                    onStatusChange={handleStatusChange}
                                />
                            </div>
                        </div>
                    </li>
                ))}
            </ul>

            {/* Pagination controls */}
            <div className="flex justify-between mt-4 w-3/4">
                <button
                    disabled={page === 0}
                    onClick={() => setPage(page - 1)}
                    className="bg-gray-500 text-white px-4 py-2 rounded disabled:bg-gray-700"
                >
                    Previous
                </button>
                <button
                    onClick={() => setPage(page + 1)}
                    className="bg-gray-500 text-white px-4 py-2 rounded"
                >
                    Next
                </button>
            </div>
            
            {/* Modal to order details */}
            {modalOpen && selectedOrder && (
                <OrderModal order={selectedOrder} onClose={handleCloseModal} />
            )}
        </div>
    );
};

export default OrdersList;
