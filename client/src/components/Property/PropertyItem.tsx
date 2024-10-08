import React from "react";
import { PropertyDto } from "../../Api";

interface PropertyItemProps {
    property: PropertyDto; // Ensure PropertyDto contains at least id and propertyName
    onEdit: (id: number) => void;
    onDelete: (id: number) => void;
}

const PropertyItem: React.FC<PropertyItemProps> = ({ property, onEdit, onDelete }) => {
    return (
        <li className="p-2 text-white bg-gray-900 rounded-lg flex justify-between items-center">
            <span>{property.propertyName}</span>
            <div className="space-x-2">
                <button
                    onClick={() => onEdit(property.id!)}
                    className="btn btn-sm btn-primary"
                >
                    Edit
                </button>
                <button
                    onClick={() => onDelete(property.id!)}
                    className="btn btn-sm btn-danger bg-red-700"
                >
                    Delete
                </button>
            </div>
        </li>
    );
};

export default PropertyItem;
