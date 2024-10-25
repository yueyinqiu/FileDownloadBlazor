console.log("Loaded");

export function downloadFromUri(fileName, uri)
{
    const a = document.createElement("a");
    a.href = uri;
    a.download = fileName;
    a.click();
    a.remove();
}

export function downloadBytes(fileName, bytes)
{
    const blob = new Blob([bytes]);
    const url = URL.createObjectURL(blob);
    downloadFromUri(fileName, url);
    URL.revokeObjectURL(url);
}

export async function downloadStream(fileName, stream)
{
    const bytes = await stream.arrayBuffer();
    downloadBytes(fileName, bytes);
}
