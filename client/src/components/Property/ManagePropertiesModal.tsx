import React, { useState, useEffect } from "react";
import { useAtom } from "jotai";
import { PropertiesAtom } from "./PropertiesAtom";
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
        if (property.id) {
            const response = await http.api.propertyUpdateProperty(property.id, property);
            setProperties((prev) =>
                prev.map((prop) => (prop.id === property.id ? response.data : prop))
            );
        } else {
            const response = await http.api.propertyCreateProduct(property);
            setProperties((prev) => [...prev, response.data]);
        }
        setEditingProperty(null);
        onClose();
    };

    const handleDelete = async (id: number) => {
        await http.api.propertyDeleteProperty(id);
        setProperties((prev) => prev.filter((prop) => prop.id !== id));
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
