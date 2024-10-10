import React, { useState } from "react";
import toast from "react-hot-toast";
import { http } from "../../http";
import { CogIcon } from '@heroicons/react/24/solid';

interface StatusChangerProps {
    orderId: number;
    currentStatus: string;
    onStatusChange: () => void;
}

const StatusChanger = ({ orderId, currentStatus, onStatusChange }: StatusChangerProps) => {
    const [status, setStatus] = useState(currentStatus);

    const handleStatusChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
        toast("Updating order list...");
        const newStatus = e.target.value;
        setStatus(newStatus);

        try {
            await http.api.orderUpdateOrderStatus(orderId, { newStatus });
            toast.success("Order status updated successfully!");
            onStatusChange();
        } catch (error) {
            if (error.response) {
                toast.error(error.response.data.message || "Failed to update order status.");
            } else {
                toast.error("An error occurred while updating the status.");
            }
            setStatus(currentStatus);
        }
    };


    return (
        <div className="relative">
            <CogIcon className="w-full h-full" />

            <select 
                value={status}
                onChange={handleStatusChange}
                className="absolute inset-0 opacity-0 cursor-pointer bg-customBlue"
            >
                <option value="pending">Pending</option>
                <option value="processing">Processing</option>
                <option value="delivered">Delivered</option>
                <option value="cancelled">Cancelled</option>
            </select>
        </div>
    );
};

export default StatusChanger;
