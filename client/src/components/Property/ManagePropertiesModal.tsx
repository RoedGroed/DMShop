import React, { useState, useEffect } from "react";
import { useAtom } from "jotai";
import { toast } from "react-hot-toast";
import { PropertiesAtom } from "../../atoms/PropertiesAtom.ts";
import { PropertyDto } from "../../Api";
import PropertyForm from "./PropertyForm";
import PropertyList from "./PropertyList";
import { http } from "../../http.ts";

interface ManagePropertiesModalProps {
    isOpen: boolean;
    onClose: () => void;
}

const ManagePropertiesModal: React.FC<ManagePropertiesModalProps> = ({ isOpen, onClose }) => {
    const [properties, setProperties] = useAtom(PropertiesAtom);
    const [editingProperty, setEditingProperty] = useState<PropertyDto | null>(null);

    useEffect(() => {
        if (isOpen) {
            const fetchProperties = async () => {
                const response = await http.api.propertyGetAllProperties();
                setProperties(response.data);
            };
            fetchProperties();
        }
    }, [isOpen, setProperties]);

    const handleSave = async (property: PropertyDto) => {
        try {
            if (property.id) {
                const response = await http.api.propertyUpdateProperty(property.id, property);
                setProperties((prev) =>
                    prev.map((prop) => (prop.id === property.id ? response.data : prop))
                );
                toast.success("Update Success!");
            } else {
                const response = await http.api.propertyCreateProduct(property);
                setProperties((prev) => [...prev, response.data]);
                toast.success("Property Created Successfully!");
            }
            setEditingProperty(null);
            onClose();
        } catch (error) {
            toast.error("Failed to save property.");
        }
    };

    const handleDelete = async (id: number) => {
        toast((t) => (
            <div>
                <p>Are you sure you want to delete this property?</p>
                <div className="mt-4 flex justify-end space-x-2">
                    <button
                        onClick={async () => {
                            toast.dismiss(t.id); // Dismiss the confirmation toast
                            try {
                                await http.api.propertyDeleteProperty(id);
                                setProperties((prev) => prev.filter((prop) => prop.id !== id));
                                toast.success("Delete Success!");
                            } catch (error) {
                                toast.error("Failed to delete property.");
                            }
                        }}
                        className="btn btn-error"
                    >
                        Yes, Delete
                    </button>
                    <button
                        onClick={() => toast.dismiss(t.id)} // Cancel deletion
                        className="btn btn-outline"
                    >
                        Cancel
                    </button>
                </div>
            </div>
        ), {
            duration: 5000,
        });
    };

    return (
        isOpen && (
            <div className="fixed inset-0 flex items-center justify-center z-50">
                <div className="fixed inset-0 bg-black opacity-30" onClick={onClose}></div>
                <div className="bg-customBlue p-6 rounded-lg shadow-lg w-1/3 relative">
                    <h2 className="text-2xl font-bold mb-4">Manage Properties</h2>
                    <button onClick={onClose} className="absolute top-2 right-2">X</button>
                    <PropertyForm property={editingProperty} onSave={handleSave} />
                    <PropertyList
                        onEdit={(id) => setEditingProperty(properties.find(prop => prop.id === id) || null)}
                        onDelete={handleDelete}
                    />
                </div>
            </div>
        )
    );
};

export default ManagePropertiesModal;
