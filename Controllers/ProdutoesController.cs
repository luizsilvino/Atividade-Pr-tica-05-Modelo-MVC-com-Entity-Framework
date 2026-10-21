
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exemplo_EF.Models;
using Exemplo_EF.Data;

public class ProdutosController : Controller
{
    private readonly Context _context;

    public ProdutosController(Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = _context.Produtos.Include(p => p.Categoria);
        return View(await produtos.ToListAsync());
    }

    public async Task<IActionResult> Details(Guid? produtoid)
    {
        if (produtoid == null)
            return NotFound();

        var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProdutoId == produtoid);

        if (produto == null)
            return NotFound();

        return View(produto);
    }

    public IActionResult Create()
    {
        ViewData["CategoriaId"] = new SelectList(
            _context.Categorias, "CategoriaId", "Nome");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ProdutoId,Nome,Descricao,Preco,CategoriaId")] Produto produto)
    {
        ModelState.Remove("Categoria");

        if (ModelState.IsValid)
        {
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoriaId"] = new SelectList(
            _context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);

        return View(produto);
    }

    public async Task<IActionResult> Edit(Guid? produtoid)
    {
        if (produtoid == null)
            return NotFound();

        var produto = await _context.Produtos.FindAsync(produtoid);

        if (produto == null)
            return NotFound();

        ViewData["CategoriaId"] = new SelectList(
            _context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);

        return View(produto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid? produtoid,
        [Bind("ProdutoId,Nome,Descricao,Preco,CategoriaId")] Produto produto)
    {
        if (produtoid != produto.ProdutoId)
            return NotFound();

        ModelState.Remove("Categoria");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto.ProdutoId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoriaId"] = new SelectList(
            _context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);

        return View(produto);
    }

    public async Task<IActionResult> Delete(Guid? produtoid)
    {
        if (produtoid == null)
            return NotFound();

        var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProdutoId == produtoid);

        if (produto == null)
            return NotFound();

        return View(produto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid? produtoid)
    {
        var produto = await _context.Produtos.FindAsync(produtoid);

        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(Guid? produtoid)
    {
        return _context.Produtos.Any(p => p.ProdutoId == produtoid);
    }
}
