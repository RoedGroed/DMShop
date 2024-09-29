﻿using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    // outline the methods for the repo

    public List<Paper> GetAllPapers();

    public List<Paper> GetAllPapersWithProperties();

    public Paper CreatePaper(Paper paper);

    public Paper DeletePaper(int id);

    public Paper UpdatePaper(Paper paper, List<int> propertyIds);
    void AddPropertiesToPaper(int paperId, List<int> propertyIds);
}