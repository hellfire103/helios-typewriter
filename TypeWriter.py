import os
import tkinter as tk
from tkinter import *
import tkinter.filedialog
import tkinter.messagebox
from tkinter.font import Font

PROGRAM_NAME = "Helios TypeWriter"
file_name = None

root = Tk()
text= tk.Text(root)
root.geometry('750x500')
root.title(PROGRAM_NAME)


myFont = Font(family="FreeMono", size=14)
text.configure(font=myFont)

boldvar = '0'
italicvar = '0'
underlinevar = '0'
strikethroughvar = '0'


def show_popup_menu(event):
    popup_menu.tk_popup(event.x_root, event.y_root)


def show_cursor_info_bar():
    show_cursor_info_checked = show_cursor_info.get()
    if show_cursor_info_checked:
        cursor_info_bar.pack(expand='no', fill=None, side='right', anchor='se')
    else:
        cursor_info_bar.pack_forget()


def update_cursor_info_bar(event=None):
    row, col = content_text.index(INSERT).split('.')
    line_num, col_num = str(int(row)), str(int(col) + 1)
    infotext = "Line: {0} | Column: {1}".format(line_num, col_num)
    cursor_info_bar.config(text=infotext)


def change_theme(event=None):
    selected_theme = theme_choice.get()
    fg_bg_colors = color_schemes.get(selected_theme)
    foreground_color, background_color = fg_bg_colors.split('.')
    content_text.config(
        background=background_color, fg=foreground_color)


def update_line_numbers(event=None):
    line_numbers = get_line_numbers()
    line_number_bar.config(state='normal')
    line_number_bar.delete('1.0', 'end')
    line_number_bar.insert('1.0', line_numbers)
    line_number_bar.config(state='disabled')


def highlight_line(interval=100):
    content_text.tag_remove("active_line", 1.0, "end")
    content_text.tag_add(
        "active_line", "insert linestart", "insert lineend+1c")
    content_text.after(interval, toggle_highlight)


def undo_highlight():
    content_text.tag_remove("active_line", 1.0, "end")


def toggle_highlight(event=None):
    if to_highlight_line.get():
        highlight_line()
    else:
        undo_highlight()


def on_content_changed(event=None):
    update_line_numbers()
    update_cursor_info_bar()


def get_line_numbers():
    output = ''
    if show_line_number.get():
        row, col = content_text.index("end").split('.')
        for i in range(1, int(row)):
            output += str(i) + '\n'
    return output


def display_about_messagebox(event=None):
    tkinter.messagebox.showinfo(
        "About", "{}{}".format(PROGRAM_NAME, " 2.0\nCopyright © SlateTech 2019\nAll rights reserved."))
    

def exit_editor(event=None):
    if tkinter.messagebox.askokcancel("Quit?", "Are you sure you want to quit?\nMake sure you've saved your work."):
        root.destroy()


def new_file(event=None):
    root.title("Untitled - Helios TypeWriter")
    global file_name
    file_name = None
    content_text.delete(1.0, END)
    on_content_changed()

def clear_text(event=None):
    content_text.delete(1.0, END)
    on_content_changed()

def open_file(event=None):
    input_file_name = tkinter.filedialog.askopenfilename(defaultextension=".txt",
                                                         filetypes=[("Text Documents", "*.txt"), ("HTML Documents", "*.html"), ("Cascading Stylesheets", "*.css"), ("Python Files", "*.py *.pyw"), ("All Files", "*.*")])
    if input_file_name:
        global file_name
        file_name = input_file_name
        root.title('{} - {}'.format(os.path.basename(file_name), PROGRAM_NAME))
        content_text.delete(1.0, END)
        with open(file_name) as _file:
            content_text.insert(1.0, _file.read())
        on_content_changed()


def write_to_file(file_name):
    try:
        content = content_text.get(1.0, 'end')
        with open(file_name, 'w') as the_file:
            the_file.write(content)
    except IOError:
        tkinter.messagebox.showwarning("Save", "Could not save the file.")


def save_as(event=None):
    input_file_name = tkinter.filedialog.asksaveasfilename(defaultextension=".txt",
                                                           filetypes=[("Text Documents", "*.txt"), ("HTML Documents", "*.html"), ("Cascading Stylesheets", "*.css"), ("Python Files", "*.py *.pyw"), ("All Files", "*.*")])
    if input_file_name:
        global file_name
        file_name = input_file_name
        write_to_file(file_name)
        root.title('{} - {}'.format(os.path.basename(file_name), PROGRAM_NAME))
    return "break"


def save(event=None):
    global file_name
    if not file_name:
        save_as()
    else:
        write_to_file(file_name)
    return "break"


def select_all(event=None):
    content_text.tag_add('sel', '1.0', 'end')
    return "break"


def find_text(event=None):
    search_toplevel = Toplevel(root)
    search_toplevel.title('Find Text')
    search_toplevel.transient(root)

    Label(search_toplevel, text="Find All:").grid(row=0, column=0, sticky='e')

    search_entry_widget = Entry(
        search_toplevel, width=25)
    search_entry_widget.grid(row=0, column=1, padx=2, pady=2, sticky='we')
    search_entry_widget.focus_set()
    ignore_case_value = IntVar()
    Checkbutton(search_toplevel, text='Ignore Case', variable=ignore_case_value).grid(
        row=1, column=1, sticky='e', padx=2, pady=2)
    Button(search_toplevel, text="Find All", underline=0,
           command=lambda: search_output(
               search_entry_widget.get(), ignore_case_value.get(),
               content_text, search_toplevel, search_entry_widget)
           ).grid(row=0, column=2, sticky='e' + 'w', padx=2, pady=2)

    def close_search_window():
        content_text.tag_remove('match', '1.0', END)
        search_toplevel.destroy()
    search_toplevel.protocol('WM_DELETE_WINDOW', close_search_window)
    return "break"


def search_output(needle, if_ignore_case, content_text,
                  search_toplevel, search_box):
    content_text.tag_remove('match', '1.0', END)
    matches_found = 0
    if needle:
        start_pos = '1.0'
        while True:
            start_pos = content_text.search(needle, start_pos,
                                            nocase=if_ignore_case, stopindex=END)
            if not start_pos:
                break
            end_pos = '{}+{}c'.format(start_pos, len(needle))
            content_text.tag_add('match', start_pos, end_pos)
            matches_found += 1
            start_pos = end_pos
        content_text.tag_config(
            'match', foreground='red', background='yellow')
    search_box.focus_set()
    search_toplevel.title('{} matches found'.format(matches_found))


def cut():
    content_text.event_generate("<<Cut>>")
    on_content_changed()
    return "break"


def copy():
    content_text.event_generate("<<Copy>>")
    return "break"


def paste():
    content_text.event_generate("<<Paste>>")
    on_content_changed()
    return "break"


def undo():
    content_text.event_generate("<<Undo>>")
    on_content_changed()
    return "break"


def redo(event=None):
    content_text.event_generate("<<Redo>>")
    on_content_changed()
    return 'break'

def serif():
    myFont.configure(family='Liberation Serif')
    return 'break'

def sans_serif():
    myFont.configure(family='Liberation Sans')
    return 'break'

def monospace():
    myFont.configure(family='Liberation Mono')
    return 'break'

def decorative():
    myFont.configure(family='jr!hand')
    return 'break'

def typewriter():
    myFont.configure(family='FreeMono')
    return 'break'

def eight():
    myFont.configure(size=8)
    return 'break'

def ten():
    myFont.configure(size=10)
    return 'break'

def twelve():
    myFont.configure(size=12)
    return 'break'

def fourteen():
    myFont.configure(size=14)
    return 'break'

def sixteen():
    myFont.configure(size=16)
    return 'break'

def eighteen():
    myFont.configure(size=18)
    return 'break'

def twenty():
    myFont.configure(size=20)
    return 'break'

def twenty_four():
    myFont.configure(size=24)
    return 'break'

def twenty_eight():
    myFont.configure(size=28)
    return 'break'

def thirty_six():
    myFont.configure(size=36)
    return 'break'

def forty_eight():
    myFont.configure(size=48)
    return 'break'

def seventy_two():
    myFont.configure(size=72)
    return 'break'

def bold(event=None):
    boldvar_checked = boldvar.get()
    if boldvar_checked:
        myFont.configure(weight='bold')
        text.configure(font=myFont)
    else:
        myFont.configure(weight='normal')
        text.configure(font=myFont)

def italic():
    italicvar_checked = italicvar.get()
    if italicvar_checked:
        myFont.configure(slant='italic')
        text.configure(font=myFont)
    else:
        myFont.configure(slant='roman')
        text.configure(font=myFont)

def underline():
    underlinevar_checked = underlinevar.get()
    if underlinevar_checked:
        myFont.configure(underline=1)
        text.configure(font=myFont)
    else:
        myFont.configure(underline=0)
        text.configure(font=myFont)

def strikethrough():
    strikethroughvar_checked = strikethroughvar.get()
    if strikethroughvar_checked:
        myFont.configure(overstrike=1)
        text.configure(font=myFont)
    else:
        myFont.configure(overstrike=0)
        text.configure(font=myFont)

new_file_icon = PhotoImage(file='icons/new_file.png')
open_file_icon = PhotoImage(file='icons/open_file.png')
save_file_icon = PhotoImage(file='icons/save.png')
save_as_file_icon = PhotoImage(file='icons/save_as.png')
cut_icon = PhotoImage(file='icons/cut.png')
copy_icon = PhotoImage(file='icons/copy.png')
paste_icon = PhotoImage(file='icons/paste.png')
undo_icon = PhotoImage(file='icons/undo.png')
redo_icon = PhotoImage(file='icons/redo.png')
typewriter_icon = PhotoImage(file='icons/typewriter.png')
serif_icon = PhotoImage(file='icons/serif.png')
sans_icon = PhotoImage(file='icons/sans_serif.png')
monospace_icon = PhotoImage(file='icons/monospace.png')
decorative_icon = PhotoImage(file='icons/decorative.png')

menu_bar = Menu(root)
file_menu = Menu(menu_bar, tearoff=0)
file_menu.add_command(label='New', accelerator='Ctrl+N', compound='left',
                      image=new_file_icon, underline=0, command=new_file)
file_menu.add_command(label='Open', accelerator='Ctrl+O', compound='left',
                      image=open_file_icon, underline=0, command=open_file)
file_menu.add_command(label='Save', accelerator='Ctrl+S',
                      compound='left', image=save_file_icon, underline=0, command=save)
file_menu.add_command(label='Save as...', accelerator='Alt+S',
                      compound='left', image=save_as_file_icon, underline=0, command=save_as)
file_menu.add_separator()
file_menu.add_command(label='Exit', accelerator='Alt+F4', command=exit_editor)
menu_bar.add_cascade(label='File', menu=file_menu)

edit_menu = Menu(menu_bar, tearoff=0)
edit_menu.add_command(label='Undo', accelerator='Ctrl+Z',
                      compound='left', image=undo_icon, command=undo)
edit_menu.add_command(label='Redo', accelerator='Ctrl+Y',
                      compound='left', image=redo_icon, command=redo)
edit_menu.add_separator()
edit_menu.add_command(label='Cut', accelerator='Ctrl+X',
                      compound='left', image=cut_icon, command=cut)
edit_menu.add_command(label='Copy', accelerator='Ctrl+C',
                      compound='left', image=copy_icon, command=copy)
edit_menu.add_command(label='Paste', accelerator='Ctrl+V',
                      compound='left', image=paste_icon, command=paste)
edit_menu.add_separator()
edit_menu.add_command(label='Find', underline=0,
                      accelerator='Ctrl+F', command=find_text)
edit_menu.add_separator()
edit_menu.add_command(label='Select All', underline=7,
                      accelerator='Ctrl+A', command=select_all)
menu_bar.add_cascade(label='Edit', menu=edit_menu)


view_menu = Menu(menu_bar, tearoff=0)
show_line_number = IntVar()
show_line_number.set(0)
view_menu.add_checkbutton(label='Show Line Number', variable=show_line_number,
                          command=update_line_numbers)
show_cursor_info = IntVar()
show_cursor_info.set(1)
view_menu.add_checkbutton(
    label='Show Cursor Location at Bottom', variable=show_cursor_info, command=show_cursor_info_bar)
to_highlight_line = BooleanVar()
view_menu.add_checkbutton(label='Highlight Current Line', onvalue=1,
                          offvalue=0, variable=to_highlight_line, command=toggle_highlight)
themes_menu = Menu(menu_bar, tearoff=0)
view_menu.add_cascade(label='Themes', menu=themes_menu)

color_schemes = {
    'Default': '#000000.#FFFFFF',
    'Grey': '#83406A.#D1D4D1',
    'Marine': '#5B8340.#D1E7E0',
    'Sands': '#4B4620.#FFF0E1',
    'Cobalt': '#ffffBB.#0a0aab',
    'Athena': '#D1E7E0.#6daa09',
    'Night Mode': '#FFFFFF.#595959',
    'Homebrew': '#15ff00.#000000',
    'Christmas': '#00cc00.#000099',
}

theme_choice = StringVar()
theme_choice.set('Default')
for k in sorted(color_schemes):
    themes_menu.add_radiobutton(
        label=k, variable=theme_choice, command=change_theme)

format_menu = Menu(menu_bar, tearoff=0)

format_menu = Menu(menu_bar, tearoff=0)
font_menu = Menu(menu_bar, tearoff=0)
format_menu.add_cascade(label='Font', menu=font_menu)
fontvar = IntVar()
fontvar.set(1)
font_menu.add_radiobutton(label='Typewriter', compound='left', variable=fontvar,
                          value=1, image=typewriter_icon, command=typewriter)
font_menu.add_radiobutton(label='Serif', compound='left', variable=fontvar,
                          value=2, image=serif_icon, command=serif)
font_menu.add_radiobutton(label='Sans-Serif', compound='left', variable=fontvar,
                          value=3, image=sans_icon, command=sans_serif)
font_menu.add_radiobutton(label='Monospace', compound='left', variable=fontvar,
                          value=4, image=monospace_icon, command=monospace)
font_menu.add_radiobutton(label='Handwriting', compound='left', variable=fontvar,
                          value=5, image=decorative_icon, command=decorative)

size_menu = Menu(menu_bar, tearoff=0)
format_menu.add_cascade(label='Size', menu=size_menu)
sizevar = IntVar()
sizevar.set(4)
size_menu.add_radiobutton(label='8', command=eight, variable=sizevar, value=1)
size_menu.add_radiobutton(label='10', command=ten, variable=sizevar, value=2)
size_menu.add_radiobutton(label='12', command=twelve, variable=sizevar, value=3)
size_menu.add_radiobutton(label='14', command=fourteen, variable=sizevar, value=4)
size_menu.add_radiobutton(label='16', command=sixteen, variable=sizevar, value=5)
size_menu.add_radiobutton(label='18', command=eighteen, variable=sizevar, value=6)
size_menu.add_radiobutton(label='20', command=twenty, variable=sizevar, value=7)
size_menu.add_radiobutton(label='24', command=twenty_four, variable=sizevar, value=8)
size_menu.add_radiobutton(label='28', command=twenty_eight, variable=sizevar, value=9)
size_menu.add_radiobutton(label='36', command=thirty_six, variable=sizevar, value=10)
size_menu.add_radiobutton(label='48', command=forty_eight, variable=sizevar, value=11)
size_menu.add_radiobutton(label='72', command=seventy_two, variable=sizevar, value=12)

style_menu = Menu(menu_bar, tearoff=0)
format_menu.add_cascade(label='Style', menu=style_menu)
boldvar = IntVar()
boldvar.set(0)
style_menu.add_checkbutton(label='Bold', variable=boldvar,
                          onvalue=1, offvalue=0, command=bold)
italicvar = IntVar()
italicvar.set(0)
style_menu.add_checkbutton(label='Italic', variable=italicvar,
                           onvalue=1, offvalue=0, command=italic)
underlinevar = IntVar()
underlinevar.set(0)
style_menu.add_checkbutton(label='Underline', variable=underlinevar,
                           onvalue=1, offvalue=0, command=underline)
strikethroughvar = IntVar()
strikethroughvar.set(0)
style_menu.add_checkbutton(label='Strikethrough', variable=strikethroughvar,
                           onvalue=1, offvalue=0, command=strikethrough)
menu_bar.add_cascade(label='Format', menu=format_menu)

menu_bar.add_cascade(label='View', menu=view_menu)

about_menu = Menu(menu_bar, tearoff=0)
about_menu.add_command(label='About', command=display_about_messagebox)
menu_bar.add_cascade(label='About',  menu=about_menu)
root.config(menu=menu_bar)

shortcut_bar = Frame(root, height=25, background='Light Gray')
shortcut_bar.pack(expand='no', fill='x')

icons = ('new_file', 'open_file', 'save', 'save_as', 'cut', 'copy', 'paste',
         'undo', 'redo', 'find_text')
for i, icon in enumerate(icons):
    tool_bar_icon = PhotoImage(file='icons/{}.png'.format(icon))
    cmd = eval(icon)
    tool_bar = Button(shortcut_bar, image=tool_bar_icon, command=cmd)
    tool_bar.image = tool_bar_icon
    tool_bar.pack(side='left')


line_number_bar = Text(root, width=4, padx=3, takefocus=0,  border=0,
                       background='Light Gray', state='disabled',  wrap='none')
line_number_bar.pack(side='left',  fill='y')

content_text = Text(root, wrap='word', undo=1, font=myFont, spacing1=3, selectbackground='light blue')
content_text.pack(expand='yes', fill='both')
scroll_bar = Scrollbar(content_text)
content_text.configure(yscrollcommand=scroll_bar.set)
scroll_bar.config(command=content_text.yview)
scroll_bar.pack(side='right', fill='y')
cursor_info_bar = Label(content_text, text='Line: 1 | Column: 1')
cursor_info_bar.pack(expand='no', fill=None, side='right', anchor='se')


content_text.bind('<Control-N>', new_file)
content_text.bind('<Control-n>', new_file)
content_text.bind('<Control-O>', open_file)
content_text.bind('<Control-o>', open_file)
content_text.bind('<Control-S>', save)
content_text.bind('<Control-s>', save)
content_text.bind('<Alt-S>', save_as)
content_text.bind('<Alt-s>', save_as)
content_text.bind('<Control-f>', find_text)
content_text.bind('<Control-F>', find_text)
content_text.bind('<Control-A>', select_all)
content_text.bind('<Control-a>', select_all)
content_text.bind('<Control-y>', redo)
content_text.bind('<Control-Y>', redo)
content_text.bind('<Any-KeyPress>', on_content_changed)
content_text.tag_configure('active_line', background='ivory2')

popup_menu = Menu(content_text)
popup_menu.add_command(label='Cut', underline=7, command=cut)
popup_menu.add_command(label='Copy', underline=7, command=copy)
popup_menu.add_command(label='Paste', underline=7, command=paste)
popup_menu.add_command(label='Clear', underline=7, command=clear_text)
popup_menu.add_separator()
popup_menu.add_command(label='Select All', underline=7, command=select_all)
popup_menu.add_command(label='Find Text', underline=7, command=find_text)
content_text.bind('<Button-3>', show_popup_menu)


content_text.bind('<Button-3>', show_popup_menu)
content_text.focus_set()

root.protocol('WM_DELETE_WINDOW', exit_editor)
root.mainloop()
